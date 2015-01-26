Imports System.Threading
Imports System.Windows.Forms
Imports Rhino.Geometry
Imports Rhino
Imports Rhino.PlugIns

Namespace SampleVbDigitizer

  ''' <summary>
  ''' SampleVbDigitizerPlugIn digitizer plug-in
  ''' </summary>
  Public Class SampleVbDigitizerPlugIn
    Inherits DigitizerPlugIn

    Shared _instance As SampleVbDigitizerPlugIn

    Private _thread As Thread
    Private _cancelThread As ManualResetEvent
    Private _threadisCanceled As ManualResetEvent

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Public Sub New()
      _instance = Me
    End Sub

    ''' <summary>
    ''' Gets the only instance of the SampleVbDigitizerPlugIn plug-in.
    ''' </summary>
    Public Shared ReadOnly Property Instance() As SampleVbDigitizerPlugIn
      Get
        Return _instance
      End Get
    End Property

    ''' <summary>
    ''' Called when the plug-in is being loaded.
    ''' </summary>
    Protected Overrides Function OnLoad(ByRef errorMessage As String) As LoadReturnCode
      Return LoadReturnCode.Success
    End Function

    ''' <summary>
    ''' Called when Rhino is shutting down.
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub OnShutdown()
      EnableDigitizer(False)
    End Sub

    '''<summary>
    ''' Defines the behavior in response to a request of a user to either enable or disable input from the digitizer.
    ''' This is called by Rhino. If enable is true and EnableDigitizer() returns false, then Rhino will not calibrate the digitizer.
    '''</summary>
    Protected Overrides Function EnableDigitizer(enable As Boolean) As Boolean

      If (enable) Then

        _thread = New Thread(AddressOf ThreadProcedure)
        _cancelThread = New ManualResetEvent(False)
        _threadisCanceled = New ManualResetEvent(False)

        ' Starts the new thread
        _cancelThread.Reset()
        _threadisCanceled.Reset()
        _thread.Start()

      Else

        ' Set _cancelThread to ask the thread to stop
        _cancelThread.Set()
        If _threadisCanceled.WaitOne(4000, False) Then
          ' TODO...
        End If

      End If

      Return True

    End Function

    '''<summary>
    ''' Gets the unit system in which the digitizer works.
    ''' Rhino uses this value when it calibrates a digitizer.
    ''' This unit system must not change.
    ''' </summary>
    Protected Overrides ReadOnly Property DigitizerUnitSystem() As UnitSystem
      Get
        ' TODO: Return the digitizer's unit system.
        Return UnitSystem.Millimeters
      End Get
    End Property

    ''' <summary>
    ''' Gets the point tolerance, or the distance the digitizer must move (in digitizer
    ''' coordinates) for a new point to be considered real rather than noise. Small
    ''' desktop digitizer arms have values like 0.001 inches and 0.01 millimeters.
    ''' This value should never be smaller than the accuracy of the digitizing device.
    ''' </summary>
    Protected Overrides ReadOnly Property PointTolerance() As Double
      Get
        ' TODO: Return the digitizer's point tolerance.
        Return 0.01
      End Get
    End Property

    ''' <summary>
    ''' Our thread procedure
    ''' </summary>
    Private Sub ThreadProcedure()

      While Not _cancelThread.WaitOne(0, False)
        ' Do some kind of task here.
        SendDigitizerPoint()
      End While

      If _cancelThread.WaitOne(0, False) Then
        _threadisCanceled.Set()
      End If

    End Sub

    ''' <summary>
    ''' Query digitizer and send point to Rhino
    ''' </summary>
    Private Sub SendDigitizerPoint()

      ' TODO: add whatever code you need to query your digitizer for a 3D point here.

      ' If the digitizer is enabled, call this function to send a point to Rhino. 
      ' Call this function as much as you like. The digitizers that Rhino currently
      ' supports send a point every 15 milliseconds or so. This function should be
      ' called when users press or release any digitizer button.
      SendPoint(New Point3d(0, 0, 0), MouseButtons.Left, False, False)

    End Sub

  End Class

End Namespace