using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using StarFoxZeroLocalizationTool.Models;

namespace StarFoxZeroLocalizationTool.Services
{
    public static class McdIO
    {
        #region Reading (Binary -> Model)
        public static McdFile ReadMcd(string filePath)
        {
            byte[] data = File.ReadAllBytes(filePath);
            var mcd = new McdFile();

            // Detect endianness
            uint valBe = ReadUInt32(data, 0, true);
            uint valLe = ReadUInt32(data, 0, false);
            bool isBigEndian = valBe < valLe;
            mcd.Endian = isBigEndian ? "be" : "le";

            // Header
            uint offsetEvents = ReadUInt32(data, 0, isBigEndian);
            uint eventCount = ReadUInt32(data, 4, isBigEndian);
            uint offsetCharSet = ReadUInt32(data, 8, isBigEndian);
            uint charCount = ReadUInt32(data, 12, isBigEndian);
            uint offsetCharGraphs = ReadUInt32(data, 16, isBigEndian);
            uint charGraphsCount = ReadUInt32(data, 20, isBigEndian);
            uint offsetSpecialGraphs = ReadUInt32(data, 24, isBigEndian);
            uint specialGraphsCount = ReadUInt32(data, 28, isBigEndian);
            uint offsetUsedEvents = ReadUInt32(data, 32, isBigEndian);
            uint usedEventCount = ReadUInt32(data, 36, isBigEndian);

            // Read CharSet
            for (uint i = 0; i < charCount; i++)
            {
                uint off = offsetCharSet + i * 8;
                ushort langFlags = ReadUInt16(data, off, isBigEndian);
                ushort cVal = ReadUInt16(data, off + 2, isBigEndian);
                uint index = ReadUInt32(data, off + 4, isBigEndian);

                string charUtf8;
                try
                {
                    charUtf8 = char.ConvertFromUtf32(cVal);
                }
                catch
                {
                    charUtf8 = $"[ErrorChar:{cVal}]";
                }

                mcd.Chars.Add(new CharEntry
                {
                    Id = (int)i,
                    Char = charUtf8,
                    CharCode = cVal,
                    LanguageFlags = langFlags,
                    Index = (int)index
                });
            }

            // Read CharGraphs
            for (uint i = 0; i < charGraphsCount; i++)
            {
                uint off = offsetCharGraphs + i * 40;
                mcd.CharGraphs.Add(new CharGraph
                {
                    Id = (int)i,
                    TextureID = ReadUInt32(data, off, isBigEndian).ToString("X8"),
                    U1 = ReadSingle(data, off + 4, isBigEndian),
                    V1 = ReadSingle(data, off + 8, isBigEndian),
                    U2 = ReadSingle(data, off + 12, isBigEndian),
                    V2 = ReadSingle(data, off + 16, isBigEndian),
                    Width = ReadSingle(data, off + 20, isBigEndian),
                    Height = ReadSingle(data, off + 24, isBigEndian),
                    Ua = ReadSingle(data, off + 28, isBigEndian),
                    BelowSpacing = ReadSingle(data, off + 32, isBigEndian),
                    HorizontalSpacing = ReadSingle(data, off + 36, isBigEndian)
                });
            }

            // Read SpecialGraphs
            for (uint i = 0; i < specialGraphsCount; i++)
            {
                uint off = offsetSpecialGraphs + i * 20;
                mcd.SpecialGraphs.Add(new SpecialGraph
                {
                    Id = (int)i,
                    LanguageFlags = (int)ReadUInt32(data, off, isBigEndian),
                    Width = ReadSingle(data, off + 4, isBigEndian),
                    Height = ReadSingle(data, off + 8, isBigEndian),
                    BelowSpacing = ReadSingle(data, off + 12, isBigEndian),
                    HorizontalSpacing = ReadSingle(data, off + 16, isBigEndian)
                });
            }

            // Read UsedEvents
            for (uint i = 0; i < usedEventCount; i++)
            {
                uint off = offsetUsedEvents + i * 40;
                uint eventId = ReadUInt32(data, off, isBigEndian);
                uint eventIndex = ReadUInt32(data, off + 4, isBigEndian);
                byte[] nameBytes = new byte[32];
                Array.Copy(data, off + 8, nameBytes, 0, 32);
                int nameLen = Array.FindIndex(nameBytes, b => b == 0);
                string name = Encoding.UTF8.GetString(nameBytes, 0, nameLen == -1 ? 32 : nameLen);

                mcd.UsedEvents.Add(new UsedEvent
                {
                    EventID = eventId.ToString("X8"),
                    EventIndex = (int)eventIndex,
                    Name = name
                });
            }

            // Read Events, Paragraphs, Strings
            for (uint i = 0; i < eventCount; i++)
            {
                uint off = offsetEvents + i * 16;
                uint paragraphsOffset = ReadUInt32(data, off, isBigEndian);
                uint paragraphCount = ReadUInt32(data, off + 4, isBigEndian);
                uint sequenceNumber = ReadUInt32(data, off + 8, isBigEndian);
                uint eventId = ReadUInt32(data, off + 12, isBigEndian);

                var ev = new Event
                {
                    Id = (int)i,
                    EventID = eventId.ToString("X8"),
                    SequenceNumber = (int)sequenceNumber
                };

                for (uint j = 0; j < paragraphCount; j++)
                {
                    uint pOff = paragraphsOffset + j * 20;
                    uint stringsOffset = ReadUInt32(data, pOff, isBigEndian);
                    uint stringCount = ReadUInt32(data, pOff + 4, isBigEndian);
                    float belowSpacing = ReadSingle(data, pOff + 8, isBigEndian);
                    float horizontalSpacing = ReadSingle(data, pOff + 12, isBigEndian);
                    ushort languageFlags = ReadUInt16(data, pOff + 16, isBigEndian);

                    var p = new Paragraph
                    {
                        Id = (int)j,
                        BelowSpacing = belowSpacing,
                        HorizontalSpacing = horizontalSpacing,
                        LanguageFlags = languageFlags
                    };

                    for (uint k = 0; k < stringCount; k++)
                    {
                        uint sOff = stringsOffset + k * 24;
                        uint stringOffset = ReadUInt32(data, sOff, isBigEndian);
                        uint uA = ReadUInt32(data, sOff + 4, isBigEndian);
                        uint length = ReadUInt32(data, sOff + 8, isBigEndian);
                        uint length2 = ReadUInt32(data, sOff + 12, isBigEndian);
                        float sBelowSpacing = ReadSingle(data, sOff + 16, isBigEndian);
                        float sHorizontalSpacing = ReadSingle(data, sOff + 20, isBigEndian);

                        var s = new StringEntry
                        {
                            Id = (int)k,
                            Ua = (int)uA,
                            BelowSpacing = sBelowSpacing,
                            HorizontalSpacing = sHorizontalSpacing,
                            Length = (int)length,
                            Length2 = (int)length2
                        };

                        // Read letters
                        int numLetters = ((int)length - 1) / 2;
                        var rawText = new StringBuilder();
                        for (int l = 0; l < numLetters; l++)
                        {
                            uint letOff = stringOffset + (uint)l * 4;
                            ushort code = ReadUInt16(data, letOff, isBigEndian);
                            short posOffset = ReadInt16(data, letOff + 2, isBigEndian);

                            string decoded;
                            if (code <= 0x8000)
                            {
                                var c = mcd.Chars.FirstOrDefault(x => x.Id == code);
                                decoded = c != null ? c.Char : $"[ErrorChar:{code}]";
                            }
                            else if (code == 0x8001)
                            {
                                decoded = " ";
                            }
                            else if (code == 0x8003)
                            {
                                string buttonName = posOffset switch
                                {
                                    0 => "+",
                                    1 => "-",
                                    2 => "B",
                                    3 => "A",
                                    4 => "Y",
                                    5 => "X",
                                    6 => "R",
                                    8 => "L",
                                    11 => "DPadUpDown",
                                    12 => "DPadLeftRight",
                                    17 => "RightStick",
                                    18 => "RightStickPress",
                                    19 => "LeftStick",
                                    20 => "LeftStickPress",
                                    24 => "RightStickRotate",
                                    25 => "LeftStickUpDown",
                                    113 => "SwapWeapons",
                                    114 => "Evade",
                                    115 => "UmbranClimax",
                                    116 => "LockOn",
                                    _ => posOffset.ToString()
                                };
                                decoded = $"{{button:{buttonName}}}";
                            }
                            else
                            {
                                decoded = $"{{special:0x{code & 0xff:X}_{posOffset}}}";
                            }

                            rawText.Append(decoded);
                            s.Letters.Add(new Letter { Code = code, PositionOffset = posOffset });
                        }

                        s.Terminator = ReadUInt16(data, stringOffset + (uint)numLetters * 4, isBigEndian);
                        s.Text = rawText.ToString();

                        p.Strings.Add(s);
                    }

                    ev.Paragraphs.Add(p);
                }

                mcd.Events.Add(ev);
            }

            return mcd;
        }
        #endregion

        #region Writing (Model -> Binary)
        public static void WriteMcd(McdFile mcd, string filePath)
        {
            bool isBigEndian = mcd.Endian == "be";
            var ms = new MemoryStream();

            // 1. Rebuild letters array for each string dynamically from text
            foreach (var ev in mcd.Events)
            {
                foreach (var p in ev.Paragraphs)
                {
                    foreach (var s in p.Strings)
                    {
                        var origLetters = s.Letters;
                        var newLetters = TextToLetters(s.Text, mcd.Chars, origLetters, p.LanguageFlags);
                        s.Letters = newLetters;
                        s.Length = newLetters.Count * 2 + 1;
                        s.Length2 = s.Length;
                    }
                }
            }

            // 2. Build string data
            var stringData = new MemoryStream();
            var stringOffsets = new Dictionary<(int, int, int), uint>();

            for (int evIdx = 0; evIdx < mcd.Events.Count; evIdx++)
            {
                var ev = mcd.Events[evIdx];
                for (int pIdx = 0; pIdx < ev.Paragraphs.Count; pIdx++)
                {
                    var p = ev.Paragraphs[pIdx];
                    for (int sIdx = 0; sIdx < p.Strings.Count; sIdx++)
                    {
                        var s = p.Strings[sIdx];
                        uint absOffset = 40 + (uint)stringData.Length;
                        stringOffsets[(evIdx, pIdx, sIdx)] = absOffset;

                        foreach (var let in s.Letters)
                        {
                            WriteUInt16(stringData, (ushort)let.Code, isBigEndian);
                            WriteInt16(stringData, (short)let.PositionOffset, isBigEndian);
                        }
                        WriteUInt16(stringData, (ushort)s.Terminator, isBigEndian);
                    }
                }
            }

            // Align string data to 4 bytes
            int paddingLen = (4 - ((int)stringData.Length % 4)) % 4;
            for (int i = 0; i < paddingLen; i++) stringData.WriteByte(0);

            // 3. Calculate offsets for other sections
            uint offsetEvents = 40 + (uint)stringData.Length;
            int eventCount = mcd.Events.Count;

            uint totalParagraphs = 0;
            foreach (var ev in mcd.Events) totalParagraphs += (uint)ev.Paragraphs.Count;
            uint offsetParagraphs = offsetEvents + (uint)eventCount * 16 + 4;

            uint totalStrings = 0;
            foreach (var ev in mcd.Events)
                foreach (var p in ev.Paragraphs)
                    totalStrings += (uint)p.Strings.Count;
            uint offsetStrings = offsetParagraphs + totalParagraphs * 20 + 4;

            // 4. Build events, paragraphs, strings bins
            var eventsBin = new MemoryStream();
            var paragraphsBin = new MemoryStream();
            var stringsBin = new MemoryStream();
            uint currentParagraphOffset = offsetParagraphs;
            uint currentStringOffset = offsetStrings;

            for (int evIdx = 0; evIdx < mcd.Events.Count; evIdx++)
            {
                var ev = mcd.Events[evIdx];
                WriteUInt32(eventsBin, currentParagraphOffset, isBigEndian);
                WriteUInt32(eventsBin, (uint)ev.Paragraphs.Count, isBigEndian);
                WriteUInt32(eventsBin, (uint)ev.SequenceNumber, isBigEndian);
                WriteUInt32(eventsBin, uint.Parse(ev.EventID, System.Globalization.NumberStyles.HexNumber), isBigEndian);

                for (int pIdx = 0; pIdx < ev.Paragraphs.Count; pIdx++)
                {
                    var p = ev.Paragraphs[pIdx];
                    WriteUInt32(paragraphsBin, currentStringOffset, isBigEndian);
                    WriteUInt32(paragraphsBin, (uint)p.Strings.Count, isBigEndian);
                    WriteSingle(paragraphsBin, p.BelowSpacing, isBigEndian);
                    WriteSingle(paragraphsBin, p.HorizontalSpacing, isBigEndian);
                    WriteUInt16(paragraphsBin, (ushort)p.LanguageFlags, isBigEndian);
                    WriteUInt16(paragraphsBin, 0, isBigEndian); // padding

                    currentParagraphOffset += 20;

                    for (int sIdx = 0; sIdx < p.Strings.Count; sIdx++)
                    {
                        var s = p.Strings[sIdx];
                        uint absStrOffset = stringOffsets[(evIdx, pIdx, sIdx)];
                        WriteUInt32(stringsBin, absStrOffset, isBigEndian);
                        WriteUInt32(stringsBin, (uint)s.Ua, isBigEndian);
                        WriteUInt32(stringsBin, (uint)s.Length, isBigEndian);
                        WriteUInt32(stringsBin, (uint)s.Length2, isBigEndian);
                        WriteSingle(stringsBin, s.BelowSpacing, isBigEndian);
                        WriteSingle(stringsBin, s.HorizontalSpacing, isBigEndian);

                        currentStringOffset += 24;
                    }
                }
            }

            WriteUInt32(eventsBin, 0, isBigEndian);
            WriteUInt32(paragraphsBin, 0, isBigEndian);
            WriteUInt32(stringsBin, 0, isBigEndian);

            // 5. Charset bin
            uint offsetCharSet = offsetStrings + (uint)stringsBin.Length;
            int charCount = mcd.Chars.Count;
            var charsetBin = new MemoryStream();
            foreach (var c in mcd.Chars)
            {
                WriteUInt16(charsetBin, (ushort)c.LanguageFlags, isBigEndian);
                WriteUInt16(charsetBin, (ushort)c.CharCode, isBigEndian);
                WriteUInt32(charsetBin, (uint)c.Index, isBigEndian);
            }
            WriteUInt32(charsetBin, 0, isBigEndian);

            // 6. CharGraphs bin
            uint offsetCharGraphs = offsetCharSet + (uint)charsetBin.Length;
            int charGraphsCount = mcd.CharGraphs.Count;
            var chargraphsBin = new MemoryStream();
            foreach (var g in mcd.CharGraphs)
            {
                WriteUInt32(chargraphsBin, uint.Parse(g.TextureID, System.Globalization.NumberStyles.HexNumber), isBigEndian);
                WriteSingle(chargraphsBin, g.U1, isBigEndian);
                WriteSingle(chargraphsBin, g.V1, isBigEndian);
                WriteSingle(chargraphsBin, g.U2, isBigEndian);
                WriteSingle(chargraphsBin, g.V2, isBigEndian);
                WriteSingle(chargraphsBin, g.Width, isBigEndian);
                WriteSingle(chargraphsBin, g.Height, isBigEndian);
                WriteSingle(chargraphsBin, g.Ua, isBigEndian);
                WriteSingle(chargraphsBin, g.BelowSpacing, isBigEndian);
                WriteSingle(chargraphsBin, g.HorizontalSpacing, isBigEndian);
            }
            WriteUInt32(chargraphsBin, 0, isBigEndian);

            // 7. SpecialGraphs bin
            uint offsetSpecialGraphs = offsetCharGraphs + (uint)chargraphsBin.Length;
            int specialGraphsCount = mcd.SpecialGraphs.Count;
            var specialgraphsBin = new MemoryStream();
            foreach (var g in mcd.SpecialGraphs)
            {
                WriteUInt32(specialgraphsBin, (uint)g.LanguageFlags, isBigEndian);
                WriteSingle(specialgraphsBin, g.Width, isBigEndian);
                WriteSingle(specialgraphsBin, g.Height, isBigEndian);
                WriteSingle(specialgraphsBin, g.BelowSpacing, isBigEndian);
                WriteSingle(specialgraphsBin, g.HorizontalSpacing, isBigEndian);
            }
            WriteUInt32(specialgraphsBin, 0, isBigEndian);

            // 8. UsedEvents bin
            uint offsetUsedEvents = offsetSpecialGraphs + (uint)specialgraphsBin.Length;
            int usedEventCount = mcd.UsedEvents.Count;
            var usedeventsBin = new MemoryStream();
            foreach (var e in mcd.UsedEvents)
            {
                WriteUInt32(usedeventsBin, uint.Parse(e.EventID, System.Globalization.NumberStyles.HexNumber), isBigEndian);
                WriteUInt32(usedeventsBin, (uint)e.EventIndex, isBigEndian);
                byte[] nameBytes = Encoding.UTF8.GetBytes(e.Name);
                Array.Resize(ref nameBytes, 32);
                usedeventsBin.Write(nameBytes, 0, 32);
            }

            // 9. Header
            var headerBin = new MemoryStream();
            WriteUInt32(headerBin, offsetEvents, isBigEndian);
            WriteUInt32(headerBin, (uint)eventCount, isBigEndian);
            WriteUInt32(headerBin, offsetCharSet, isBigEndian);
            WriteUInt32(headerBin, (uint)charCount, isBigEndian);
            WriteUInt32(headerBin, offsetCharGraphs, isBigEndian);
            WriteUInt32(headerBin, (uint)charGraphsCount, isBigEndian);
            WriteUInt32(headerBin, offsetSpecialGraphs, isBigEndian);
            WriteUInt32(headerBin, (uint)specialGraphsCount, isBigEndian);
            WriteUInt32(headerBin, offsetUsedEvents, isBigEndian);
            WriteUInt32(headerBin, (uint)usedEventCount, isBigEndian);

            // Combine all
            headerBin.WriteTo(ms);
            stringData.WriteTo(ms);
            eventsBin.WriteTo(ms);
            paragraphsBin.WriteTo(ms);
            stringsBin.WriteTo(ms);
            charsetBin.WriteTo(ms);
            chargraphsBin.WriteTo(ms);
            specialgraphsBin.WriteTo(ms);
            usedeventsBin.WriteTo(ms);

            File.WriteAllBytes(filePath, ms.ToArray());
        }
        #endregion

        #region Helpers: Text <-> Letters
        public static List<Letter> BuildPreviewLetters(
            string text,
            McdFile mcd,
            List<Letter> originalLetters,
            int paragraphLanguageFlags)
        {
            if (mcd == null)
            {
                throw new ArgumentNullException(nameof(mcd));
            }

            return TextToLetters(text, mcd.Chars, originalLetters ?? new List<Letter>(), paragraphLanguageFlags);
        }

        private static List<Letter> TextToLetters(string text, List<CharEntry> charset, List<Letter> originalLetters, int paragraphLanguageFlags)
        {
            var defaultSpaceOffset = GetDefaultSpaceOffset(originalLetters);
            var preferredCharIdsByValue = BuildPreferredCharacterIds(originalLetters, charset);
            var preferredLanguageFlags = BuildPreferredLanguageFlags(originalLetters, charset);
            var newTokens = ParseTextTokens(text, charset, defaultSpaceOffset, preferredCharIdsByValue, preferredLanguageFlags, paragraphLanguageFlags);
            if (originalLetters == null || originalLetters.Count == 0)
            {
                var rebuiltLetters = newTokens
                    .Select(token => new Letter { Code = token.Code, PositionOffset = token.PositionOffset })
                    .ToList();

                NormalizeCharacterLetterCodes(rebuiltLetters, newTokens, charset, preferredCharIdsByValue, preferredLanguageFlags, paragraphLanguageFlags);
                return rebuiltLetters;
            }

            var originalTokens = BuildOriginalTokens(originalLetters, charset);
            var consumedOriginalIndexes = new bool[originalTokens.Count];
            var result = new Letter[newTokens.Count];

            // Align unchanged token subsequences first so repeated characters keep their nearest original offsets.
            foreach (var (originalIndex, newIndex) in FindBestTokenMatches(originalTokens, newTokens))
            {
                result[newIndex] = new Letter
                {
                    Code = originalTokens[originalIndex].Code,
                    PositionOffset = originalTokens[originalIndex].PositionOffset
                };
                consumedOriginalIndexes[originalIndex] = true;
            }

            // Reuse the remaining original offsets by token signature in encounter order.
            var remainingOriginalBySignature = new Dictionary<string, Queue<TokenData>>();
            for (var i = 0; i < originalTokens.Count; i++)
            {
                if (consumedOriginalIndexes[i])
                {
                    continue;
                }

                var signature = BuildTokenSignature(originalTokens[i]);
                if (!remainingOriginalBySignature.TryGetValue(signature, out var queue))
                {
                    queue = new Queue<TokenData>();
                    remainingOriginalBySignature[signature] = queue;
                }

                queue.Enqueue(originalTokens[i]);
            }

            for (var i = 0; i < newTokens.Count; i++)
            {
                if (result[i] != null)
                {
                    continue;
                }

                var token = newTokens[i];
                var signature = BuildTokenSignature(token);
                if (remainingOriginalBySignature.TryGetValue(signature, out var queue) && queue.Count > 0)
                {
                    var originalToken = queue.Dequeue();
                    result[i] = new Letter
                    {
                        Code = originalToken.Code,
                        PositionOffset = originalToken.PositionOffset
                    };
                }
                else
                {
                    result[i] = new Letter
                    {
                        Code = token.Code,
                        PositionOffset = token.PositionOffset
                    };
                }
            }

            var normalizedResult = result.ToList();
            NormalizeCharacterLetterCodes(normalizedResult, newTokens, charset, preferredCharIdsByValue, preferredLanguageFlags, paragraphLanguageFlags);
            return normalizedResult;
        }

        private static List<(int OriginalIndex, int NewIndex)> FindBestTokenMatches(
            List<TokenData> originalTokens,
            List<TokenData> newTokens)
        {
            var originalCount = originalTokens.Count;
            var newCount = newTokens.Count;
            var lcs = new int[originalCount + 1, newCount + 1];

            for (var i = originalCount - 1; i >= 0; i--)
            {
                for (var j = newCount - 1; j >= 0; j--)
                {
                    if (BuildTokenSignature(originalTokens[i]) == BuildTokenSignature(newTokens[j]))
                    {
                        lcs[i, j] = lcs[i + 1, j + 1] + 1;
                    }
                    else
                    {
                        lcs[i, j] = Math.Max(lcs[i + 1, j], lcs[i, j + 1]);
                    }
                }
            }

            var matches = new List<(int OriginalIndex, int NewIndex)>();
            var origIndex = 0;
            var newIndex = 0;

            while (origIndex < originalCount && newIndex < newCount)
            {
                if (BuildTokenSignature(originalTokens[origIndex]) == BuildTokenSignature(newTokens[newIndex]))
                {
                    matches.Add((origIndex, newIndex));
                    origIndex++;
                    newIndex++;
                }
                else if (lcs[origIndex + 1, newIndex] >= lcs[origIndex, newIndex + 1])
                {
                    origIndex++;
                }
                else
                {
                    newIndex++;
                }
            }

            return matches;
        }

        private static List<TokenData> ParseTextTokens(
            string text,
            List<CharEntry> charset,
            int defaultSpaceOffset,
            Dictionary<string, List<int>> preferredCharIdsByValue,
            List<int> preferredLanguageFlags,
            int paragraphLanguageFlags)
        {
            var tokens = new List<TokenData>();
            var i = 0;

            while (i < text.Length)
            {
                if (text[i] == '{')
                {
                    if (i + 1 < text.Length && text.Substring(i).StartsWith("{button:", StringComparison.Ordinal))
                    {
                        var endIdx = text.IndexOf('}', i);
                        if (endIdx != -1)
                        {
                            var buttonName = text.Substring(i + 8, endIdx - (i + 8));
                            tokens.Add(new TokenData(TokenKind.Button, buttonName, 0x8003, ButtonNameToOffset(buttonName)));
                            i = endIdx + 1;
                            continue;
                        }
                    }
                    else if (i + 1 < text.Length && text.Substring(i).StartsWith("{special:", StringComparison.Ordinal))
                    {
                        var endIdx = text.IndexOf('}', i);
                        if (endIdx != -1)
                        {
                            var content = text.Substring(i + 9, endIdx - (i + 9));
                            var parts = content.Split('_');
                            if (parts.Length == 2 && parts[0].StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                            {
                                var rawCode = Convert.ToInt32(parts[0], 16);
                                var fullCode = rawCode >= 0x8000 ? rawCode : 0x8000 | rawCode;
                                var posOffset = int.Parse(parts[1]);
                                tokens.Add(new TokenData(TokenKind.Special, content, fullCode, posOffset));
                                i = endIdx + 1;
                                continue;
                            }
                        }
                    }
                }

                var currentChar = text[i].ToString();
                if (text[i] == ' ')
                {
                    tokens.Add(new TokenData(TokenKind.Space, " ", 0x8001, defaultSpaceOffset));
                }
                else
                {
                    var found = FindBestCharEntry(currentChar, charset, preferredCharIdsByValue, preferredLanguageFlags, paragraphLanguageFlags);
                    if (found == null)
                    {
                        throw new Exception($"Character '{text[i]}' not found in charset!");
                    }

                    tokens.Add(new TokenData(TokenKind.Character, currentChar, found.Id, 0));
                }

                i++;
            }

            return tokens;
        }

        private static CharEntry? FindBestCharEntry(
            string value,
            List<CharEntry> charset,
            Dictionary<string, List<int>> preferredCharIdsByValue,
            List<int> preferredLanguageFlags,
            int paragraphLanguageFlags)
        {
            var candidates = charset
                .Where(x => x.Char == value)
                .OrderBy(x => x.Index)
                .ThenBy(x => x.Id)
                .ToList();

            if (candidates.Count == 0)
            {
                return null;
            }

            var paragraphMatches = candidates
                .Where(x => x.LanguageFlags == paragraphLanguageFlags)
                .ToList();

            if (paragraphMatches.Count > 0)
            {
                if (preferredCharIdsByValue.TryGetValue(value, out var paragraphPreferredIds))
                {
                    foreach (var preferredId in paragraphPreferredIds)
                    {
                        var exactParagraphMatch = paragraphMatches.FirstOrDefault(x => x.Id == preferredId);
                        if (exactParagraphMatch != null)
                        {
                            return exactParagraphMatch;
                        }
                    }
                }

                return paragraphMatches[0];
            }

            if (preferredCharIdsByValue.TryGetValue(value, out var preferredIds))
            {
                foreach (var preferredId in preferredIds)
                {
                    var exactMatch = candidates.FirstOrDefault(x => x.Id == preferredId);
                    if (exactMatch != null)
                    {
                        return exactMatch;
                    }
                }
            }

            foreach (var languageFlag in preferredLanguageFlags)
            {
                var languageMatch = candidates.FirstOrDefault(x => x.LanguageFlags == languageFlag);
                if (languageMatch != null)
                {
                    return languageMatch;
                }
            }

            return candidates[0];
        }

        private static void NormalizeCharacterLetterCodes(
            List<Letter> letters,
            List<TokenData> newTokens,
            List<CharEntry> charset,
            Dictionary<string, List<int>> preferredCharIdsByValue,
            List<int> preferredLanguageFlags,
            int paragraphLanguageFlags)
        {
            for (var i = 0; i < letters.Count && i < newTokens.Count; i++)
            {
                var token = newTokens[i];
                if (token.Kind != TokenKind.Character)
                {
                    continue;
                }

                var currentEntry = charset.FirstOrDefault(x => x.Id == letters[i].Code);
                var hasParagraphMatch = charset.Any(x => x.Char == token.Value && x.LanguageFlags == paragraphLanguageFlags);
                if (currentEntry != null &&
                    currentEntry.Char == token.Value &&
                    (!hasParagraphMatch || currentEntry.LanguageFlags == paragraphLanguageFlags))
                {
                    continue;
                }

                var preferredEntry = FindBestCharEntry(
                    token.Value,
                    charset,
                    preferredCharIdsByValue,
                    preferredLanguageFlags,
                    paragraphLanguageFlags);

                if (preferredEntry != null)
                {
                    letters[i].Code = preferredEntry.Id;
                }
            }
        }

        private static int GetDefaultSpaceOffset(List<Letter> originalLetters)
        {
            if (originalLetters == null || originalLetters.Count == 0)
            {
                return 0;
            }

            return originalLetters
                .Where(letter => letter.Code == 0x8001)
                .GroupBy(letter => letter.PositionOffset)
                .OrderByDescending(group => group.Count())
                .ThenBy(group => group.Key)
                .Select(group => group.Key)
                .FirstOrDefault();
        }

        private static Dictionary<string, List<int>> BuildPreferredCharacterIds(List<Letter> originalLetters, List<CharEntry> charset)
        {
            if (originalLetters == null || originalLetters.Count == 0)
            {
                return new Dictionary<string, List<int>>();
            }

            return originalLetters
                .Where(letter => letter.Code > 0 && letter.Code < 0x8000)
                .Select(letter =>
                {
                    var entry = charset.FirstOrDefault(x => x.Id == letter.Code);
                    return new { Letter = letter, Entry = entry };
                })
                .Where(x => x.Entry != null && !string.IsNullOrEmpty(x.Entry.Char))
                .GroupBy(x => x.Entry!.Char)
                .ToDictionary(
                    group => group.Key!,
                    group => group
                        .GroupBy(x => x.Letter.Code)
                        .OrderByDescending(codeGroup => codeGroup.Count())
                        .ThenBy(codeGroup => codeGroup.Key)
                        .Select(codeGroup => codeGroup.Key)
                        .ToList());
        }

        private static List<int> BuildPreferredLanguageFlags(List<Letter> originalLetters, List<CharEntry> charset)
        {
            if (originalLetters == null || originalLetters.Count == 0)
            {
                return new List<int>();
            }

            var entries = originalLetters
                .Where(letter => letter.Code > 0 && letter.Code < 0x8000)
                .Select(letter => charset.FirstOrDefault(x => x.Id == letter.Code))
                .OfType<CharEntry>()
                .ToList();

            var letterEntries = entries
                .Where(entry => !string.IsNullOrEmpty(entry.Char) && entry.Char.Any(char.IsLetter))
                .ToList();

            var sourceEntries = letterEntries.Count > 0 ? letterEntries : entries;
            return sourceEntries
                .GroupBy(entry => entry.LanguageFlags)
                .OrderByDescending(group => group.Count())
                .ThenBy(group => group.Key)
                .Select(group => group.Key)
                .ToList();
        }

        private static List<TokenData> BuildOriginalTokens(List<Letter> originalLetters, List<CharEntry> charset)
        {
            var tokens = new List<TokenData>(originalLetters.Count);
            foreach (var letter in originalLetters)
            {
                if (letter.Code <= 0x8000)
                {
                    var entry = charset.FirstOrDefault(x => x.Id == letter.Code);
                    var value = entry?.Char ?? $"[ErrorChar:{letter.Code}]";
                    tokens.Add(new TokenData(TokenKind.Character, value, letter.Code, letter.PositionOffset));
                }
                else if (letter.Code == 0x8001)
                {
                    tokens.Add(new TokenData(TokenKind.Space, " ", letter.Code, letter.PositionOffset));
                }
                else if (letter.Code == 0x8003)
                {
                    tokens.Add(new TokenData(TokenKind.Button, ButtonOffsetToName(letter.PositionOffset), letter.Code, letter.PositionOffset));
                }
                else
                {
                    var value = $"0x{letter.Code & 0xff:X}_{letter.PositionOffset}";
                    tokens.Add(new TokenData(TokenKind.Special, value, letter.Code, letter.PositionOffset));
                }
            }

            return tokens;
        }

        private static string BuildTokenSignature(TokenData token)
        {
            return token.Kind switch
            {
                TokenKind.Character => $"char:{token.Value}",
                TokenKind.Space => "space",
                TokenKind.Button => $"button:{token.Value}",
                TokenKind.Special => $"special:{token.Value}",
                _ => token.Value
            };
        }

        private static int ButtonNameToOffset(string buttonName)
        {
            return buttonName switch
            {
                "+" => 0,
                "-" => 1,
                "B" => 2,
                "A" => 3,
                "Y" => 4,
                "X" => 5,
                "R" => 6,
                "L" => 8,
                "DPadUpDown" => 11,
                "DPadLeftRight" => 12,
                "RightStick" => 17,
                "RightStickPress" => 18,
                "LeftStick" => 19,
                "LeftStickPress" => 20,
                "RightStickRotate" => 24,
                "LeftStickUpDown" => 25,
                "SwapWeapons" => 113,
                "Evade" => 114,
                "UmbranClimax" => 115,
                "LockOn" => 116,
                _ => int.TryParse(buttonName, out var parsedValue) ? parsedValue : 0
            };
        }

        private static string ButtonOffsetToName(int positionOffset)
        {
            return positionOffset switch
            {
                0 => "+",
                1 => "-",
                2 => "B",
                3 => "A",
                4 => "Y",
                5 => "X",
                6 => "R",
                8 => "L",
                11 => "DPadUpDown",
                12 => "DPadLeftRight",
                17 => "RightStick",
                18 => "RightStickPress",
                19 => "LeftStick",
                20 => "LeftStickPress",
                24 => "RightStickRotate",
                25 => "LeftStickUpDown",
                113 => "SwapWeapons",
                114 => "Evade",
                115 => "UmbranClimax",
                116 => "LockOn",
                _ => positionOffset.ToString()
            };
        }

        private enum TokenKind
        {
            Character,
            Space,
            Button,
            Special
        }

        private sealed class TokenData
        {
            public TokenData(TokenKind kind, string value, int code, int positionOffset)
            {
                Kind = kind;
                Value = value;
                Code = code;
                PositionOffset = positionOffset;
            }

            public TokenKind Kind { get; }

            public string Value { get; }

            public int Code { get; }

            public int PositionOffset { get; }
        }

        public static List<string> ValidateTextCharacters(string text, List<CharEntry> charset)
        {
            var missing = new List<string>();
            int i = 0;
            while (i < text.Length)
            {
                if (text[i] == '{')
                {
                    if (text.Substring(i).StartsWith("{button:") || text.Substring(i).StartsWith("{special:"))
                    {
                        int endIdx = text.IndexOf('}', i);
                        if (endIdx != -1)
                        {
                            i = endIdx + 1;
                            continue;
                        }
                    }
                }
                char c = text[i];
                if (c != ' ')
                {
                    var found = charset.FirstOrDefault(x => x.Char == c.ToString());
                    if (found == null)
                    {
                        if (!missing.Contains(c.ToString()))
                            missing.Add(c.ToString());
                    }
                }
                i++;
            }
            return missing;
        }
        #endregion

        #region Binary Primitive Helpers
        private static uint ReadUInt32(byte[] data, long offset, bool isBigEndian)
        {
            byte[] b = new byte[4];
            Array.Copy(data, offset, b, 0, 4);
            if (isBigEndian) Array.Reverse(b);
            return BitConverter.ToUInt32(b, 0);
        }

        private static ushort ReadUInt16(byte[] data, long offset, bool isBigEndian)
        {
            byte[] b = new byte[2];
            Array.Copy(data, offset, b, 0, 2);
            if (isBigEndian) Array.Reverse(b);
            return BitConverter.ToUInt16(b, 0);
        }

        private static short ReadInt16(byte[] data, long offset, bool isBigEndian)
        {
            byte[] b = new byte[2];
            Array.Copy(data, offset, b, 0, 2);
            if (isBigEndian) Array.Reverse(b);
            return BitConverter.ToInt16(b, 0);
        }

        private static float ReadSingle(byte[] data, long offset, bool isBigEndian)
        {
            byte[] b = new byte[4];
            Array.Copy(data, offset, b, 0, 4);
            if (isBigEndian) Array.Reverse(b);
            return BitConverter.ToSingle(b, 0);
        }

        private static void WriteUInt32(MemoryStream ms, uint value, bool isBigEndian)
        {
            byte[] b = BitConverter.GetBytes(value);
            if (isBigEndian) Array.Reverse(b);
            ms.Write(b, 0, 4);
        }

        private static void WriteUInt16(MemoryStream ms, ushort value, bool isBigEndian)
        {
            byte[] b = BitConverter.GetBytes(value);
            if (isBigEndian) Array.Reverse(b);
            ms.Write(b, 0, 2);
        }

        private static void WriteInt16(MemoryStream ms, short value, bool isBigEndian)
        {
            byte[] b = BitConverter.GetBytes(value);
            if (isBigEndian) Array.Reverse(b);
            ms.Write(b, 0, 2);
        }

        private static void WriteSingle(MemoryStream ms, float value, bool isBigEndian)
        {
            byte[] b = BitConverter.GetBytes(value);
            if (isBigEndian) Array.Reverse(b);
            ms.Write(b, 0, 4);
        }
        #endregion
    }
}
