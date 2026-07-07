using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerysBitStream.Generator.Emission;

internal sealed class SourceWriter {
    private readonly StringBuilder _builder = new();
    private int _indent;

    public int Indent {
        get => _indent;
        set => _indent = Math.Max(0, value);
    }

    public void WriteLine(string text = "") {
        if (text.Length != 0) {
            _builder.Append(' ', Indent * 4);
            _builder.Append(text);
        }
        _builder.AppendLine();
    }

    public void WriteLines(string text) {
        string[] lines = text.Split(["\r\n", "\r", "\n"], StringSplitOptions.None);
        foreach (string line in lines) { WriteLine(line); }
    }

    public void WriteBlocks(IEnumerable<string> blocks) {
        bool isFirstBlock = true;
        foreach (string block in blocks) {
            if (!isFirstBlock) { WriteLine(); }
            WriteLines(block);
            isFirstBlock = false;
        }
    }

    public override string ToString() => _builder.ToString();

    public static string MaintainRelativeIndent(string text, int relativeIndent) {
        if (string.IsNullOrEmpty(text)) { return text; }
        relativeIndent = Math.Max(relativeIndent, 0);

        string[] lines = text.Split(["\r\n", "\r", "\n"], StringSplitOptions.None);
        if (lines.Length == 1) { return text; }

        StringBuilder builder = new StringBuilder(text.Length + relativeIndent * 4 * (lines.Length - 1));
        builder.Append(lines[0]);

        string padding = new(' ', relativeIndent * 4);
        for (int i = 1; i < lines.Length; i++) {
            builder.AppendLine();
            if (lines[i].Length != 0) {
                builder.Append(padding);
                builder.Append(lines[i]);
            }
        }

        return builder.ToString();
    }
}
