using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace StarFoxZeroLocalizationTool.Models
{
    public class McdFile
    {
        [JsonPropertyName("endian")]
        public string Endian { get; set; } = "be";

        [JsonPropertyName("chars")]
        public List<CharEntry> Chars { get; set; } = new();

        [JsonPropertyName("char_graphs")]
        public List<CharGraph> CharGraphs { get; set; } = new();

        [JsonPropertyName("special_graphs")]
        public List<SpecialGraph> SpecialGraphs { get; set; } = new();

        [JsonPropertyName("used_events")]
        public List<UsedEvent> UsedEvents { get; set; } = new();

        [JsonPropertyName("events")]
        public List<Event> Events { get; set; } = new();
    }

    public class CharEntry
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("char")]
        public string Char { get; set; } = "";

        [JsonPropertyName("char_code")]
        public int CharCode { get; set; }

        [JsonPropertyName("languageFlags")]
        public int LanguageFlags { get; set; }

        [JsonPropertyName("index")]
        public int Index { get; set; }
    }

    public class CharGraph
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("textureID")]
        public string TextureID { get; set; } = "";

        [JsonPropertyName("u1")]
        public float U1 { get; set; }

        [JsonPropertyName("v1")]
        public float V1 { get; set; }

        [JsonPropertyName("u2")]
        public float U2 { get; set; }

        [JsonPropertyName("v2")]
        public float V2 { get; set; }

        [JsonPropertyName("width")]
        public float Width { get; set; }

        [JsonPropertyName("height")]
        public float Height { get; set; }

        [JsonPropertyName("u_a")]
        public float Ua { get; set; }

        [JsonPropertyName("belowSpacing")]
        public float BelowSpacing { get; set; }

        [JsonPropertyName("horizontalSpacing")]
        public float HorizontalSpacing { get; set; }
    }

    public class SpecialGraph
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("languageFlags")]
        public int LanguageFlags { get; set; }

        [JsonPropertyName("width")]
        public float Width { get; set; }

        [JsonPropertyName("height")]
        public float Height { get; set; }

        [JsonPropertyName("belowSpacing")]
        public float BelowSpacing { get; set; }

        [JsonPropertyName("horizontalSpacing")]
        public float HorizontalSpacing { get; set; }
    }

    public class UsedEvent
    {
        [JsonPropertyName("eventID")]
        public string EventID { get; set; } = "";

        [JsonPropertyName("eventIndex")]
        public int EventIndex { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = "";
    }

    public class Event
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("eventID")]
        public string EventID { get; set; } = "";

        [JsonPropertyName("sequenceNumber")]
        public int SequenceNumber { get; set; }

        [JsonPropertyName("paragraphs")]
        public List<Paragraph> Paragraphs { get; set; } = new();
    }

    public class Paragraph
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("belowSpacing")]
        public float BelowSpacing { get; set; }

        [JsonPropertyName("horizontalSpacing")]
        public float HorizontalSpacing { get; set; }

        [JsonPropertyName("languageFlags")]
        public int LanguageFlags { get; set; }

        [JsonPropertyName("strings")]
        public List<StringEntry> Strings { get; set; } = new();
    }

    public class StringEntry
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; } = "";

        [JsonPropertyName("u_a")]
        public int Ua { get; set; }

        [JsonPropertyName("length")]
        public int Length { get; set; }

        [JsonPropertyName("length2")]
        public int Length2 { get; set; }

        [JsonPropertyName("belowSpacing")]
        public float BelowSpacing { get; set; }

        [JsonPropertyName("horizontalSpacing")]
        public float HorizontalSpacing { get; set; }

        [JsonPropertyName("terminator")]
        public int Terminator { get; set; }

        [JsonPropertyName("letters")]
        public List<Letter> Letters { get; set; } = new();
    }

    public class Letter
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("positionOffset")]
        public int PositionOffset { get; set; }
    }
}
