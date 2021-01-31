using System;
using System.Collections.Generic;
using Xunit;

namespace MyTextEditor.EditorBuffer
{

    public class TestOriginalText : TestEditorBufferStub
    {
        [Fact]
        public void OriginalText_CanSetString_OnlyInConstructor()
        {
            var original = new OriginalText(TextSequence);

            // compiler block original.TextSequece = "";
            Assert.Equal(TextSequence, original.Sequece);
        }
    }
}
