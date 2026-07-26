using System.Globalization;
using System.Text.Json;
using System.Windows.Forms;

namespace StarFoxZeroLocalizationTool.Localization;

internal sealed record LanguageOption(string Code, string DisplayName);

internal static class LocalizationService
{
    private const string DefaultLanguageCode = "en";
    private static readonly string LocalizationDirectory = Path.Combine(AppContext.BaseDirectory, "Localization");
    private static readonly IReadOnlyList<LanguageOption> SupportedLanguages =
    [
        new("en", "English"),
        new("pt-BR", "Português (Brasil)")
    ];

    private static readonly Dictionary<string, IReadOnlyDictionary<string, string>> CatalogCache = new(StringComparer.OrdinalIgnoreCase);
    private static IReadOnlyDictionary<string, string> _fallbackCatalog = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    private static IReadOnlyDictionary<string, string> _currentCatalog = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    public static string CurrentLanguageCode { get; private set; } = DefaultLanguageCode;

    public static IReadOnlyList<LanguageOption> Languages => SupportedLanguages;

    public static void Initialize()
    {
        _fallbackCatalog = LoadCatalog(DefaultLanguageCode);

        foreach (var language in SupportedLanguages)
        {
            LoadCatalog(language.Code);
        }

        ValidateCatalogs();

        var preferredLanguage = LanguagePreferenceStore.LoadPreferredLanguageCode();
        ApplyLanguage(preferredLanguage, persistPreference: false);
    }

    public static bool ApplyLanguage(string? languageCode, bool persistPreference = true)
    {
        var normalizedCode = NormalizeLanguageCode(languageCode);
        var changed = !string.Equals(CurrentLanguageCode, normalizedCode, StringComparison.OrdinalIgnoreCase);

        CurrentLanguageCode = normalizedCode;
        _currentCatalog = LoadCatalog(normalizedCode);

        var culture = CultureInfo.GetCultureInfo(normalizedCode);
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;

        if (persistPreference)
        {
            LanguagePreferenceStore.SavePreferredLanguageCode(normalizedCode);
        }

        return changed;
    }

    public static string Get(string key)
    {
        if (_currentCatalog.TryGetValue(key, out var localizedValue))
        {
            return localizedValue;
        }

        if (_fallbackCatalog.TryGetValue(key, out var fallbackValue))
        {
            return fallbackValue;
        }

        return $"[[{key}]]";
    }

    public static string Format(string key, params object?[] args)
    {
        return string.Format(CultureInfo.CurrentCulture, Get(key), args);
    }

    public static bool TryGet(string key, out string value)
    {
        if (_currentCatalog.TryGetValue(key, out value!))
        {
            return true;
        }

        if (_fallbackCatalog.TryGetValue(key, out value!))
        {
            return true;
        }

        value = string.Empty;
        return false;
    }

    public static void ApplyFormTexts(Form form, ISet<string>? dynamicControlNames = null, ISet<string>? dynamicToolStripNames = null)
    {
        ArgumentNullException.ThrowIfNull(form);

        dynamicControlNames ??= EmptySet.Instance;
        dynamicToolStripNames ??= EmptySet.Instance;

        ApplyFormText(form);
        ApplyControlTexts(form, form.GetType().Name, dynamicControlNames);
        ApplyOwnedToolStripTexts(form, form.GetType().Name, dynamicToolStripNames);
    }

    private static string NormalizeLanguageCode(string? languageCode)
    {
        if (string.IsNullOrWhiteSpace(languageCode))
        {
            return DefaultLanguageCode;
        }

        return SupportedLanguages
            .FirstOrDefault(language => string.Equals(language.Code, languageCode, StringComparison.OrdinalIgnoreCase))
            ?.Code
            ?? DefaultLanguageCode;
    }

    private static IReadOnlyDictionary<string, string> LoadCatalog(string languageCode)
    {
        if (CatalogCache.TryGetValue(languageCode, out var cachedCatalog))
        {
            return cachedCatalog;
        }

        var filePath = Path.Combine(LocalizationDirectory, $"{languageCode}.json");
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Localization catalog not found: {filePath}", filePath);
        }

        var json = File.ReadAllText(filePath);
        var catalog = JsonSerializer.Deserialize<Dictionary<string, string>>(json)
            ?? throw new InvalidOperationException($"Localization catalog '{languageCode}' is invalid.");

        var normalizedCatalog = new Dictionary<string, string>(catalog, StringComparer.OrdinalIgnoreCase);
        CatalogCache[languageCode] = normalizedCatalog;
        return normalizedCatalog;
    }

    private static void ValidateCatalogs()
    {
        var baseKeys = new HashSet<string>(_fallbackCatalog.Keys, StringComparer.OrdinalIgnoreCase);
        foreach (var language in SupportedLanguages)
        {
            var catalog = LoadCatalog(language.Code);
            var missingKeys = baseKeys
                .Where(key => !catalog.ContainsKey(key))
                .OrderBy(key => key, StringComparer.OrdinalIgnoreCase)
                .ToArray();

            if (missingKeys.Length > 0)
            {
                throw new InvalidOperationException(
                    $"Localization catalog '{language.Code}' is missing {missingKeys.Length} key(s): {string.Join(", ", missingKeys)}");
            }
        }
    }

    private static void ApplyFormText(Form form)
    {
        if (TryGet($"{form.GetType().Name}.$this.Text", out var text))
        {
            form.Text = text;
        }
    }

    private static void ApplyControlTexts(Control parent, string formPrefix, ISet<string> dynamicControlNames)
    {
        foreach (Control control in parent.Controls)
        {
            if (!dynamicControlNames.Contains(control.Name) && SupportsLocalizedText(control))
            {
                var key = $"{formPrefix}.{control.Name}.Text";
                if (TryGet(key, out var text))
                {
                    control.Text = text;
                }
            }

            if (control.HasChildren)
            {
                ApplyControlTexts(control, formPrefix, dynamicControlNames);
            }
        }
    }

    private static void ApplyOwnedToolStripTexts(Control parent, string formPrefix, ISet<string> dynamicToolStripNames)
    {
        foreach (ToolStrip toolStrip in parent.Controls.OfType<ToolStrip>())
        {
            ApplyToolStripItemTexts(toolStrip.Items, formPrefix, dynamicToolStripNames);
        }
    }

    private static void ApplyToolStripItemTexts(ToolStripItemCollection items, string formPrefix, ISet<string> dynamicToolStripNames)
    {
        foreach (ToolStripItem item in items)
        {
            if (!string.IsNullOrEmpty(item.Name) && !dynamicToolStripNames.Contains(item.Name))
            {
                var key = $"{formPrefix}.{item.Name}.Text";
                if (TryGet(key, out var text))
                {
                    item.Text = text;
                }
            }

            if (item is ToolStripDropDownItem dropDownItem)
            {
                ApplyToolStripItemTexts(dropDownItem.DropDownItems, formPrefix, dynamicToolStripNames);
            }
        }
    }

    private static bool SupportsLocalizedText(Control control)
    {
        return control is Label
            or LinkLabel
            or Button
            or CheckBox
            or GroupBox;
    }

    private static class EmptySet
    {
        public static readonly ISet<string> Instance = new HashSet<string>(StringComparer.Ordinal);
    }
}

internal static class Loc
{
    public static string Get(string key) => LocalizationService.Get(key);

    public static string Format(string key, params object?[] args) => LocalizationService.Format(key, args);
}

internal static class LanguagePreferenceStore
{
    private static readonly string SettingsDirectory =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "StarFoxZeroLocalizationTool");

    private static readonly string SettingsPath = Path.Combine(SettingsDirectory, "user-preferences.json");

    public static string? LoadPreferredLanguageCode()
    {
        try
        {
            if (!File.Exists(SettingsPath))
            {
                return null;
            }

            var json = File.ReadAllText(SettingsPath);
            var payload = JsonSerializer.Deserialize<UserPreferencesPayload>(json);
            return payload?.LanguageCode;
        }
        catch
        {
            return null;
        }
    }

    public static void SavePreferredLanguageCode(string languageCode)
    {
        Directory.CreateDirectory(SettingsDirectory);
        var payload = new UserPreferencesPayload
        {
            LanguageCode = languageCode
        };

        File.WriteAllText(SettingsPath, JsonSerializer.Serialize(payload, new JsonSerializerOptions
        {
            WriteIndented = true
        }));
    }

    private sealed class UserPreferencesPayload
    {
        public string LanguageCode { get; set; } = "en";
    }
}
