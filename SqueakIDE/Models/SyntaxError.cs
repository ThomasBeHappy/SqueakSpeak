namespace SqueakIDE.Models
{
    // Syntax error representation
    public class SyntaxError
    {
        public int Line { get; set; }
        public int Column { get; set; }
        public int StartIndex { get; set; }
        public int Length { get; set; }
        public string Message { get; set; }
    }
}
