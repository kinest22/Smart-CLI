using System;
using System.Collections.Generic;
using System.Globalization;
using SmartCLI;
using SmartCLI.Commands;



// new command space
var testCmdSpace = CommandSpace.ConfigureNew(cmdspace =>
{
    // command space properties
    cmdspace.HasName("Test Command Space")
    .HasDescription("This is test.");

    // adding new command to command space
    cmdspace.NewCommand<TestParams>(cmd =>
    {
        // cmd properties
        cmd.HasName("main")
        .HasDescription("this is main command.")
        .HasRoutine(para => CommandRoutine(para))
        .IsHidden();

        // arg (numeric)
        cmd.HasNumericArg(para => para.Id)
        .WithName("ID")
        .WithDescription("user ID.")
        .WithMinValue(1)
        .WithFormatProvider(CultureInfo.InvariantCulture);

        // arg (string)
        cmd.HasStringArg(para => para.Name!)
        .WithName("Name")
        .WithDescription("User name.")
        .WithMaxLength(10)
        .WithPosition(2);

        // arg (collection)
        cmd.HasCollectionArg(para => para.Values)
        .WithName("numbers")
        .WithDescription("Lucky numbers.")
        .WithAllowedValues(22.0, 7.0);
    });
});

var cmd = testCmdSpace.Commands[0];
Console.WriteLine("General cmd info:");
Console.WriteLine($"name: {cmd.Name}");
Console.WriteLine($"desc: {cmd.Description}");
Console.WriteLine("\nExecution result:");
cmd.ExecuteSolely("1 kinest22 22 7");

// target routine to execute.
static void CommandRoutine(TestParams @params)
{
    Console.WriteLine("this is cli test.");
    Console.WriteLine($"Ma name is {@params.Name}");
    Console.WriteLine($"test int is {@params.Id}");
    foreach(var val in @params.Values)
        Console.WriteLine($"\tI have value {val}");
}


/// <summary>
///     Represents test command parameters.
/// </summary>
public class TestParams : VoidParams
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public ICollection<double> Values { get; set; } = Array.Empty<double>();
}