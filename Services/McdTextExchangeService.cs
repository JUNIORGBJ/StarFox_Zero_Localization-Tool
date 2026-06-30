using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using StarFoxZeroLocalizationTool.Models;

namespace StarFoxZeroLocalizationTool.Services
{
    internal static class McdTextExchangeService
    {
        private const char CsvSeparator = ';';

        public static void ExportToCsv(McdFile mcd, string outputPath, string? sourceFilePath)
        {
            if (mcd == null)
            {
                throw new ArgumentNullException(nameof(mcd));
            }

            var entries = FlattenStrings(mcd).ToList();
            var builder = new StringBuilder();
            builder.AppendLine("sep=;");
            builder.AppendLine(string.Join(CsvSeparator, new[]
            {
                "INDEX",
                "EVENT_ID",
                "EVENT_NAME",
                "EVENT_INDEX",
                "PARAGRAPH_ID",
                "PARAGRAPH_INDEX",
                "STRING_ID",
                "STRING_INDEX",
                "TEXT_ORIGINAL",
                "TEXT_TRADUZIDO"
            }));

            foreach (var entry in entries)
            {
                builder.AppendLine(string.Join(CsvSeparator, new[]
                {
                    ToCsvField(entry.GlobalIndex.ToString()),
                    ToCsvField(entry.EventId),
                    ToCsvField(entry.EventName),
                    ToCsvField(entry.EventIndex.ToString()),
                    ToCsvField(entry.ParagraphId.ToString()),
                    ToCsvField(entry.ParagraphIndex.ToString()),
                    ToCsvField(entry.StringId.ToString()),
                    ToCsvField(entry.StringIndex.ToString()),
                    ToCsvField(entry.Entry.Text ?? string.Empty),
                    ToCsvField(string.Empty)
                }));
            }

            var directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(outputPath, builder.ToString(), Encoding.Unicode);
        }

        public static McdTextImportResult ImportFromCsv(McdFile mcd, string inputPath)
        {
            if (mcd == null)
            {
                throw new ArgumentNullException(nameof(mcd));
            }

            if (!File.Exists(inputPath))
            {
                throw new FileNotFoundException("O arquivo CSV informado nao foi encontrado.", inputPath);
            }

            var parsedCsv = ParseEntries(ReadTextWithAutoEncoding(inputPath));
            var importedEntries = parsedCsv.Entries;
            var targetEntries = FlattenStrings(mcd).ToList();
            var targetsByKey = targetEntries
                .GroupBy(entry => entry.MatchKey, StringComparer.Ordinal)
                .ToDictionary(group => group.Key, group => new Queue<McdStringLocation>(group), StringComparer.Ordinal);

            var exactMatches = 0;
            var indexFallbacks = 0;
            var unmatchedEntries = new List<int>();

            foreach (var importedEntry in importedEntries)
            {
                McdStringLocation? target = null;
                if (targetsByKey.TryGetValue(importedEntry.MatchKey, out var queue) && queue.Count > 0)
                {
                    target = queue.Dequeue();
                    exactMatches++;
                }
                else if (importedEntry.GlobalIndex >= 0 && importedEntry.GlobalIndex < targetEntries.Count)
                {
                    target = targetEntries[importedEntry.GlobalIndex];
                    indexFallbacks++;
                }

                if (target == null)
                {
                    unmatchedEntries.Add(importedEntry.GlobalIndex);
                    continue;
                }

                target.Entry.Text = importedEntry.Text;
            }

            return new McdTextImportResult(
                parsedCsv.TotalDataRows,
                exactMatches + indexFallbacks,
                exactMatches,
                indexFallbacks,
                parsedCsv.SkippedEmptyTranslatedRows,
                unmatchedEntries.Count,
                unmatchedEntries);
        }

        private static ParsedCsvEntries ParseEntries(string csvText)
        {
            var rows = ParseCsvRows(csvText);
            if (rows.Count == 0)
            {
                throw new InvalidOperationException("O CSV esta vazio.");
            }

            NormalizeFirstCell(rows);

            var rowIndex = 0;
            if (IsSeparatorDirectiveRow(rows[0]))
            {
                rowIndex++;
            }

            if (rowIndex >= rows.Count)
            {
                throw new InvalidOperationException("O CSV nao contem cabecalho.");
            }

            var header = rows[rowIndex++];
            var headerMap = BuildHeaderMap(header);
            var entries = new List<ImportedTextEntry>();
            var totalDataRows = 0;
            var skippedEmptyTranslatedRows = 0;
            for (; rowIndex < rows.Count; rowIndex++)
            {
                var row = rows[rowIndex];
                if (row.Count == 0 || row.All(string.IsNullOrWhiteSpace))
                {
                    continue;
                }

                totalDataRows++;

                var importText = ResolveImportText(row, headerMap, rowIndex + 1);
                if (!importText.ShouldApply)
                {
                    skippedEmptyTranslatedRows++;
                    continue;
                }

                entries.Add(new ImportedTextEntry(
                    GetRequiredInt(row, headerMap, "INDEX", rowIndex + 1),
                    GetRequiredString(row, headerMap, "EVENT_ID", rowIndex + 1),
                    GetRequiredInt(row, headerMap, "PARAGRAPH_ID", rowIndex + 1),
                    GetRequiredInt(row, headerMap, "STRING_ID", rowIndex + 1),
                    importText.Text));
            }

            return new ParsedCsvEntries(entries, totalDataRows, skippedEmptyTranslatedRows);
        }

        private static void NormalizeFirstCell(List<List<string>> rows)
        {
            if (rows.Count == 0 || rows[0].Count == 0)
            {
                return;
            }

            rows[0][0] = rows[0][0].TrimStart('\uFEFF');
        }

        private static bool IsSeparatorDirectiveRow(IReadOnlyList<string> row)
        {
            if (row.Count == 1)
            {
                return string.Equals(row[0].Trim(), "sep=;", StringComparison.OrdinalIgnoreCase);
            }

            return row.Count == 2
                && string.Equals(row[0].Trim(), "sep=", StringComparison.OrdinalIgnoreCase)
                && string.IsNullOrWhiteSpace(row[1]);
        }

        private static Dictionary<string, int> BuildHeaderMap(List<string> header)
        {
            var headerMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            for (var index = 0; index < header.Count; index++)
            {
                var name = header[index].Trim();
                if (!string.IsNullOrWhiteSpace(name) && !headerMap.ContainsKey(name))
                {
                    headerMap[name] = index;
                }
            }

            foreach (var requiredColumn in new[] { "INDEX", "EVENT_ID", "PARAGRAPH_ID", "STRING_ID" })
            {
                if (!headerMap.ContainsKey(requiredColumn))
                {
                    throw new InvalidOperationException($"Coluna obrigatoria ausente no CSV: {requiredColumn}.");
                }
            }

            if (!headerMap.ContainsKey("TEXT_TRADUZIDO")
                && !headerMap.ContainsKey("TEXT")
                && !headerMap.ContainsKey("TEXT_ORIGINAL"))
            {
                throw new InvalidOperationException("O CSV precisa conter pelo menos uma coluna de texto: TEXT_TRADUZIDO, TEXT ou TEXT_ORIGINAL.");
            }

            return headerMap;
        }

        private static int GetRequiredInt(IReadOnlyList<string> row, IReadOnlyDictionary<string, int> headerMap, string key, int rowNumber)
        {
            var rawValue = GetRequiredString(row, headerMap, key, rowNumber);
            if (!int.TryParse(rawValue, out var value))
            {
                throw new InvalidOperationException($"Valor numerico invalido para {key} na linha {rowNumber}.");
            }

            return value;
        }

        private static string GetRequiredString(IReadOnlyList<string> row, IReadOnlyDictionary<string, int> headerMap, string key, int rowNumber)
        {
            if (!headerMap.TryGetValue(key, out var columnIndex))
            {
                throw new InvalidOperationException($"Coluna {key} ausente no CSV.");
            }

            if (columnIndex >= row.Count)
            {
                throw new InvalidOperationException($"Campo {key} ausente na linha {rowNumber}.");
            }

            return row[columnIndex];
        }

        private static ImportTextResolution ResolveImportText(IReadOnlyList<string> row, IReadOnlyDictionary<string, int> headerMap, int rowNumber)
        {
            if (headerMap.ContainsKey("TEXT_TRADUZIDO"))
            {
                var translated = GetOptionalString(row, headerMap, "TEXT_TRADUZIDO");
                if (!string.IsNullOrWhiteSpace(translated))
                {
                    return new ImportTextResolution(true, translated);
                }

                return new ImportTextResolution(false, string.Empty);
            }

            if (headerMap.ContainsKey("TEXT"))
            {
                return new ImportTextResolution(true, GetRequiredString(row, headerMap, "TEXT", rowNumber));
            }

            return new ImportTextResolution(true, GetRequiredString(row, headerMap, "TEXT_ORIGINAL", rowNumber));
        }

        private static string? GetOptionalString(IReadOnlyList<string> row, IReadOnlyDictionary<string, int> headerMap, string key)
        {
            if (!headerMap.TryGetValue(key, out var columnIndex) || columnIndex >= row.Count)
            {
                return null;
            }

            return row[columnIndex];
        }

        private static IEnumerable<McdStringLocation> FlattenStrings(McdFile mcd)
        {
            var usedEventsById = mcd.UsedEvents
                .GroupBy(x => x.EventID, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(group => group.Key, group => group.First().Name, StringComparer.OrdinalIgnoreCase);

            var globalIndex = 0;
            for (var eventIndex = 0; eventIndex < mcd.Events.Count; eventIndex++)
            {
                var ev = mcd.Events[eventIndex];
                var eventName = ResolveEventName(ev, usedEventsById);
                for (var paragraphIndex = 0; paragraphIndex < ev.Paragraphs.Count; paragraphIndex++)
                {
                    var paragraph = ev.Paragraphs[paragraphIndex];
                    for (var stringIndex = 0; stringIndex < paragraph.Strings.Count; stringIndex++)
                    {
                        var entry = paragraph.Strings[stringIndex];
                        yield return new McdStringLocation(
                            globalIndex++,
                            ev.EventID,
                            eventName,
                            paragraph.Id,
                            entry.Id,
                            eventIndex,
                            paragraphIndex,
                            stringIndex,
                            entry);
                    }
                }
            }
        }

        private static string ToCsvField(string? value)
        {
            var normalized = (value ?? string.Empty).Replace("\r\n", "\n").Replace('\r', '\n');
            var escaped = normalized.Replace("\"", "\"\"");
            return $"\"{escaped}\"";
        }

        private static string ReadTextWithAutoEncoding(string inputPath)
        {
            var bytes = File.ReadAllBytes(inputPath);
            if (bytes.Length >= 2)
            {
                if (bytes[0] == 0xFF && bytes[1] == 0xFE)
                {
                    return Encoding.Unicode.GetString(bytes, 2, bytes.Length - 2);
                }

                if (bytes[0] == 0xFE && bytes[1] == 0xFF)
                {
                    return Encoding.BigEndianUnicode.GetString(bytes, 2, bytes.Length - 2);
                }
            }

            if (bytes.Length >= 3
                && bytes[0] == 0xEF
                && bytes[1] == 0xBB
                && bytes[2] == 0xBF)
            {
                return Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3);
            }

            // Excel antigo no Windows costuma salvar CSV local em Windows-1252.
            return Encoding.GetEncoding(1252).GetString(bytes);
        }

        private static string ResolveEventName(Event ev, IReadOnlyDictionary<string, string> usedEventsById)
        {
            if (usedEventsById.TryGetValue(ev.EventID, out var name) && !string.IsNullOrWhiteSpace(name))
            {
                return name;
            }

            return $"Evento {ev.Id}";
        }

        private static List<List<string>> ParseCsvRows(string text)
        {
            var rows = new List<List<string>>();
            var currentRow = new List<string>();
            var currentField = new StringBuilder();
            var insideQuotes = false;

            for (var index = 0; index < text.Length; index++)
            {
                var ch = text[index];

                if (insideQuotes)
                {
                    if (ch == '"')
                    {
                        if (index + 1 < text.Length && text[index + 1] == '"')
                        {
                            currentField.Append('"');
                            index++;
                        }
                        else
                        {
                            insideQuotes = false;
                        }
                    }
                    else
                    {
                        currentField.Append(ch);
                    }

                    continue;
                }

                if (ch == '"')
                {
                    insideQuotes = true;
                    continue;
                }

                if (ch == CsvSeparator)
                {
                    currentRow.Add(currentField.ToString());
                    currentField.Clear();
                    continue;
                }

                if (ch == '\r')
                {
                    continue;
                }

                if (ch == '\n')
                {
                    currentRow.Add(currentField.ToString());
                    currentField.Clear();
                    rows.Add(currentRow);
                    currentRow = new List<string>();
                    continue;
                }

                currentField.Append(ch);
            }

            if (insideQuotes)
            {
                throw new InvalidOperationException("O CSV terminou com um campo TEXT aberto entre aspas.");
            }

            if (currentField.Length > 0 || currentRow.Count > 0)
            {
                currentRow.Add(currentField.ToString());
                rows.Add(currentRow);
            }

            return rows;
        }

        internal sealed record McdTextImportResult(
            int TotalImportedEntries,
            int AppliedEntries,
            int ExactMatches,
            int IndexFallbackMatches,
            int SkippedEmptyTranslatedRows,
            int UnmatchedEntries,
            IReadOnlyList<int> UnmatchedIndexes);

        private sealed record ParsedCsvEntries(
            List<ImportedTextEntry> Entries,
            int TotalDataRows,
            int SkippedEmptyTranslatedRows);

        private sealed record ImportTextResolution(
            bool ShouldApply,
            string Text);

        private sealed record ImportedTextEntry(
            int GlobalIndex,
            string EventId,
            int ParagraphId,
            int StringId,
            string Text)
        {
            public string MatchKey => BuildMatchKey(EventId, ParagraphId, StringId);
        }

        private sealed record McdStringLocation(
            int GlobalIndex,
            string EventId,
            string EventName,
            int ParagraphId,
            int StringId,
            int EventIndex,
            int ParagraphIndex,
            int StringIndex,
            StringEntry Entry)
        {
            public string MatchKey => BuildMatchKey(EventId, ParagraphId, StringId);
        }

        private static string BuildMatchKey(string eventId, int paragraphId, int stringId)
        {
            return $"{eventId}::{paragraphId}::{stringId}";
        }
    }
}
