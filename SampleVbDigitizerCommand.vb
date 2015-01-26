Imports Rhino
Imports Rhino.Commands

Namespace SampleVbDigitizer

  ''' <summary>
  ''' SampleVbDigitizerCommand command
  ''' </summary>
  ''' <remarks></remarks>
  <System.Runtime.InteropServices.Guid("3a08d4d3-1afe-4a68-b398-62e42f6ffd7f")> _
  Public Class SampleVbDigitizerCommand
    Inherits Command

    Shared _instance As SampleVbDigitizerCommand

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Public Sub New()
      ' Rhino only creates one instance of each command class defined in a
      ' plug-in, so it is safe to store a refence in a static field.
      _instance = Me
    End Sub

    ''' <summary>
    ''' The only instance of this command.
    ''' </summary>
    Public Shared ReadOnly Property Instance() As SampleVbDigitizerCommand
      Get
        Return _instance
      End Get
    End Property

    ''' <returns>
    ''' The command name as it appears on the Rhino command line.
    ''' </returns>
    Public Overrides ReadOnly Property EnglishName() As String
      Get
        Return "SampleVbDigitizer"
      End Get
    End Property

    ''' <summary>
    ''' Called by Rhino to run this command.
    ''' </summary>
    Protected Overrides Function RunCommand(ByVal doc As RhinoDoc, ByVal mode As RunMode) As Result
      RhinoApp.WriteLine(String.Format("{0} plug-in loaded.", SampleVbDigitizerPlugIn.Instance.Name))
      Return Result.Success
    End Function

  End Class

End Namespace