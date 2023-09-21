
namespace SmartCLI.Commands
{
    internal class StgringArgument : Argument
    {
        public override string? Name { get; set; }
        public override string? Description { get; set; }
        public override int Position { get; set; }
        public string? Value { get; set; }

        public override void Parse(string strval)
        {
            Value = strval;
        }

        public override void Validate()
        {
            throw new System.NotImplementedException();
        }
    }
}
