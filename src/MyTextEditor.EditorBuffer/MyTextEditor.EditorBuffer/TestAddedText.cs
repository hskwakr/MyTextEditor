using Xunit;

namespace MyTextEditor.EditorBuffer
{
    public class TestAddedText : TestEditorBufferStub
    {
        [Fact]
        public void AddedText_CanChangeString()
        {
            var added = new AddedText(TextSequence);
            Assert.Equal(TextSequence, added.Sequece);

            added.Sequece = "";
            Assert.Equal("", added.Sequece);
        }
    }
}
