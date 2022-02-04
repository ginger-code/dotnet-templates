using System.Text;
using System.Text.RegularExpressions;
using Spectre.Console;

#region Global Variables

const string defaultProjectName      = "GingerCodeSite";
const string defaultShortProjectName = "ginger-code-site";
string[] pathFilters =
{
    "bin" ,
    "bin" ,
    "obj" ,
    "obj" ,
    "node_modules" ,
    ".idea" ,
    ".git" ,
    ".vs" ,
    ".vscode" ,
};
string projectName , shortProjectName;

#endregion

#region Messages

void Greet()
{
    AnsiConsole.Write(
        new FigletText( "ginger-code-site" ).Color(
            new Color(
                200 ,
                50 ,
                200
            )
        )
    );
    WriteCentered( "Bootstrapper for GingerCodeSite" );
}

void Starting()
{
    WriteCentered( "[blue]Updating template files![/]" );
}

void PrintFileContentUpdate( string path )
{
    AnsiConsole.MarkupLine( $"[yellow]MODIFIED CONTENTS:[/] [dim white]{path}[/]" );
}

void PrintPathChange( string oldPath , string newPath )
{
    AnsiConsole.MarkupLine(
        $"[yellow]MODIFIED PATH:[/] [dim red]{oldPath}[/] [blue]→[/] [green]{newPath}[/]"
    );
}

void Complete()
{
    WriteCentered(
        "[green bold]All done! You can now delete the bootstrapper project and scripts.[/]"
    );
}

#endregion

#region Prompts

void GetProjectName()
{
    projectName = AnsiConsole.Prompt(
        new TextPrompt< string >( "What would you like to name your project?" )
            .DefaultValue( defaultProjectName )
            .Validate(
                p =>
                {
                    try
                    {
                        if ( string.IsNullOrEmpty( p ) )
                        {
                            return ValidationResult.Error(
                                "[red]You must provide a name for the project[/]"
                            );
                        }

                        if ( Regex.IsMatch( p , @"[a-z_A-Z]\w+(?:\.?[a-z_A-Z]\w+)*" ) )
                        {
                            return ValidationResult.Error(
                                "[red]The given project name is invalid. Try to conform to the regex: \"[[a-z_A-Z]]\\w+(?:\\.?[[a-z_A-Z]]\\w+)*\"[/]"
                            );
                        }

                        return ValidationResult.Success();
                    }
                    catch ( Exception e )
                    {
                        AnsiConsole.WriteException(
                            e ,
                            ExceptionFormats.ShortenPaths
                            | ExceptionFormats.ShortenTypes
                            | ExceptionFormats.ShortenMethods
                            | ExceptionFormats.ShowLinks
                        );
                        return ValidationResult.Error(
                            "[red]The program hated that input. Try something else.[/]"
                        );
                    }
                }
            )
    );
}

void GetShortProjectName()
{
    shortProjectName = AnsiConsole.Prompt(
        new TextPrompt< string >(
                "What would you like to use as a short identifier for your project?"
            ).DefaultValue( defaultShortProjectName )
             .Validate(
                 p =>
                 {
                     try
                     {
                         if ( string.IsNullOrEmpty( p ) )
                         {
                             return ValidationResult.Error(
                                 "[red]You must provide a short identifier for the project[/]"
                             );
                         }

                         if ( Regex.IsMatch( p , @"[a-z_A-Z]\w+(?:\.?[a-z_A-Z]\w+)*" ) )
                         {
                             return ValidationResult.Error(
                                 "[red]The given project name is invalid. Try to conform to the regex: \"[[a-z_A-Z]]\\w+(?:\\-?[[a-z_A-Z]]\\w+)*\"[/]"
                             );
                         }

                         return ValidationResult.Success();
                     }
                     catch ( Exception e )
                     {
                         return ValidationResult.Error(
                             $"[red]The program hated that input. Try something else.[/]\r\n[yellow]{e}[/]"
                         );
                     }
                 }
             )
    );
}

#endregion

#region Helpers

void WriteCentered( string message )
{
    AnsiConsole.Write(
        new Rule( message )
        {
            Alignment = Justify.Center ,
            // Style     = Style.Parse( "black dim" ) ,
            Border = BoxBorder.None ,
        }
    );
}

string UpdateString( string s )
    => s.Replace(
            defaultProjectName ,
            projectName ,
            StringComparison.InvariantCultureIgnoreCase
        )
        .Replace(
            defaultShortProjectName ,
            shortProjectName ,
            StringComparison.InvariantCultureIgnoreCase
        );

string NormalizePath( string path )
    => Path.GetRelativePath( Environment.CurrentDirectory , Path.GetFullPath( path ) );

//There shouldn't be any files large enough to run out of memory doing this
void RenameInFile( string filePath )
{
    File.WriteAllText(
        filePath ,
        UpdateString( File.ReadAllText( filePath , Encoding.UTF8 ) ) ,
        Encoding.UTF8
    );
    PrintFileContentUpdate( filePath );
}

void RenameFile( string filePath )
{
    var newPath = UpdateString( NormalizePath( filePath ) );
    File.Move( filePath , newPath );
    PrintPathChange( filePath , newPath );
}

void UpdateFile( string filePath )
{
    RenameInFile( filePath );
    RenameFile( filePath );
}

void RenameDirectory( string directoryPath )
{
    var newPath = UpdateString( NormalizePath( directoryPath ) );
    Directory.Move( directoryPath , newPath );
    PrintPathChange( directoryPath , newPath );
}

bool ShouldIgnore( string path )
    => Path.GetFullPath( path )
           .Split(
               new[] { Path.PathSeparator , Path.AltDirectorySeparatorChar } ,
               StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
           )
           .Any( p => pathFilters.Contains( p ) );

bool ShouldInclude( string path )
    => !ShouldIgnore( path );

void UpdateFilesInDirectory( string directoryPath )
{
    foreach ( var file in Directory.EnumerateFiles( directoryPath )
                                   .Where( ShouldInclude ) )
    {
        UpdateFile( file );
    }
}

void UpdateSubdirectories( string directoryPath )
{
    foreach ( var subDirectory in Directory.EnumerateDirectories( directoryPath )
                                           .Where( ShouldInclude ) )
    {
        UpdateDirectory( subDirectory );
    }
}

void UpdateDirectory( string directoryPath )
{
    UpdateFilesInDirectory( directoryPath );
    RenameDirectory( directoryPath );
    UpdateSubdirectories( directoryPath );
}

void UpdateFilesAndDirectories()
{
    AnsiConsole.Status()
               .SpinnerStyle( Style.Parse( "bold yellow" ) )
               .Spinner( Spinner.Known.Aesthetic )
               .Start(
                   "Processing template files..." ,
                   _ =>
                   {
                       var path = Environment.CurrentDirectory;
                       UpdateFilesInDirectory( path );
                       var subDirectories = Directory.EnumerateDirectories( path );
                       Parallel.ForEach( subDirectories , UpdateDirectory );
                   }
               );
}

#endregion

#region Program

Greet();
GetProjectName();
GetShortProjectName();
Starting();
UpdateFilesAndDirectories();
Complete();

#endregion
