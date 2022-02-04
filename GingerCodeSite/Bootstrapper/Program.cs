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
    "Bootstrapper" ,
    "bootstrap" ,
    ".ps1" ,
    ".sh" ,
    ".fsx"
};
string projectName = "GingerCode" , shortProjectName = "ginger-code";

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
            .DefaultValue( projectName )
            .Validate(
                p =>
                {
                    try
                    {
                        if ( string.IsNullOrEmpty( p ) )
                        {
                            return ValidationResult.Success();
                        }

                        if ( !Regex.IsMatch( p , @"[a-z_A-Z]\w+(?:\.?[a-z_A-Z]\w+)*" ) )
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
            ).DefaultValue( shortProjectName )
             .Validate(
                 p =>
                 {
                     try
                     {
                         if ( string.IsNullOrEmpty( p ) )
                         {
                             return ValidationResult.Success();
                         }

                         if ( !Regex.IsMatch( p , @"[a-z_A-Z]\w+(?:\.?[a-z_A-Z]\w+)*" ) )
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
    PrintFileContentUpdate( filePath );
    File.WriteAllText(
        filePath ,
        UpdateString( File.ReadAllText( filePath , Encoding.UTF8 ) ) ,
        Encoding.UTF8
    );
}

void RenameFile( string filePath )
{
    if ( !RequiresUpdate( filePath ) ) return;
    var newPath = UpdateString( NormalizePath( filePath ) );
    if ( Path.GetFullPath( filePath ) == Path.GetFullPath( newPath ) )
        return;
    PrintPathChange( filePath , newPath );
    File.Move( filePath , newPath );
}

void UpdateFile( string filePath )
{
    if ( ShouldIgnore( filePath ) ) return;
    RenameInFile( filePath );
    RenameFile( filePath );
}

string RenameDirectory( string directoryPath )
{
    if ( !RequiresUpdate( directoryPath ) ) return directoryPath;
    var newPath = UpdateString( NormalizePath( directoryPath ) );
    if ( Path.GetFullPath( directoryPath ) == Path.GetFullPath( newPath ) )
        return directoryPath;
    PrintPathChange( directoryPath , newPath );
    Directory.Move( directoryPath , newPath );
    return newPath;
}

bool ShouldIgnore( string path )
    => pathFilters.Any(
        s => path.Contains(
                 s + Path.DirectorySeparatorChar ,
                 StringComparison.InvariantCultureIgnoreCase
             )
             || path.Contains(
                 s + Path.AltDirectorySeparatorChar ,
                 StringComparison.InvariantCultureIgnoreCase
             )
             || path.StartsWith( s )
             || path.EndsWith( s )
    );

bool ShouldInclude( string path )
    => !ShouldIgnore( path );

bool RequiresUpdate( string str )
    => str.Contains( defaultProjectName , StringComparison.InvariantCultureIgnoreCase )
       || str.Contains( defaultShortProjectName , StringComparison.InvariantCultureIgnoreCase );

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
    if ( ShouldIgnore( directoryPath ) ) return;
    var newPath = RenameDirectory( directoryPath );
    UpdateSubdirectories( newPath );
    UpdateFilesInDirectory( newPath );
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
                       UpdateDirectory( path );
                   }
               );
}

#endregion

#region Program

//
// foreach ( var s in Directory.EnumerateDirectories( Environment.CurrentDirectory )   )
// {
//     RenameDirectory( s );
//     Console.WriteLine( s );
// }
// Console.WriteLine(  );
Greet();
GetProjectName();
GetShortProjectName();
Starting();
UpdateFilesAndDirectories();
Complete();

#endregion
