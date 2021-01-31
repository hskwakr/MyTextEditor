using System;
using System.Collections.Generic;
using Xunit;

namespace MyTextEditor.EditorBuffer
{
    public abstract class TestEditorBufferStub
    {
        public readonly string TextSequence;
        public TestEditorBufferStub()
        {
            TextSequence =
                "Hello, nice to meet you.";
        }
    }
}
