Imports System.Drawing
Imports System.Threading.Tasks

' Antwoin Davis
' Code: Paint Practice
' Description: This code lets the user paint in grid tiles. 


Public Class Form1
    'View Port
    Dim TileSize As Integer = 32

    'Graphic Variables
    ' G is used to draw on the BBG. The BBG will then draw a copy of itself to BB. BB then draws itself to the screen.

    Dim G As Graphics
    Dim BBG As Graphics 'Back Buffer Graphic
    Dim BB As Bitmap 'Back Buffer
    Dim r As Rectangle

    'FPS Counter
    Dim tSec As Integer = TimeOfDay.Second
    Dim tTicks As Integer = 0
    Dim MaxTicks As Integer = 0

    'Map Variables
    Dim Map(100, 100, 10) As Integer
    Dim MapX As Integer = 20
    Dim MapY As Integer = 20

    ' Paint Brush Variabe
    Dim paintBrush As Integer

    'GAME RUNNING
    Dim isRunning As Boolean = True

    Dim mouseX As Integer
    Dim mouseY As Integer
    Dim mMapX As Integer
    Dim mMapY As Integer


    Private Sub Form1_load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Show()
        Me.Focus()

        'Initialize Graphic Objects

        G = Me.CreateGraphics
        BB = New Bitmap(Me.Width, Me.Height)

        GameController()
    End Sub

    Private Sub GameController()
        Do While isRunning = True
            'Check for user input
            Application.DoEvents()

            ' Update Obj Data
            TickCounter()

            ' Draw Graphics
            DrawGraphics()
        Loop
    End Sub
    Private Sub DrawGraphics()
        'Fill the BackBuffer
        'Draw Tiles
        For x = 0 To 19
            For y = 0 To 14
                r = New Rectangle(x * TileSize, y * TileSize, TileSize, TileSize)

                Select Case Map(MapX + x, MapY + y, 0)
                    Case 0 'Normal
                        G.FillRectangle(Brushes.BurlyWood, r)
                    Case 1 'Red
                        G.FillRectangle(Brushes.Red, r)
                    Case 2 'Blue
                        G.FillRectangle(Brushes.Blue, r)
                End Select

                G.DrawRectangle(Pens.Black, r)
            Next
        Next
        'Draw Final Layers
        'Draw Heli, Menus
        G.FillRectangle(Brushes.Red, 21 * TileSize, 4 * TileSize, TileSize, TileSize)
        G.FillRectangle(Brushes.Blue, 21 * TileSize, 6 * TileSize, TileSize, TileSize)

        G.DrawRectangle(Pens.Red, mouseX * TileSize, mouseY * TileSize, TileSize, TileSize)

        G.DrawString("Ticks: " & tTicks & vbCrLf & _
                      "TPS: " & MaxTicks & vbCrLf & _
                      "Mouse X: " & mouseX & vbCrLf & _
                      "Mouse Y: " & mouseY & vbCrLf & _
                      "M Map X: " & mMapX & vbCrLf & _
                      "M Map Y: " & mMapY, Me.Font, Brushes.Black, 650, 0)
        'Copy BackBuffer to Graphics Object
        G = Graphics.FromImage(BB)

        'Draw Backbuffer to Screen
        BBG = Me.CreateGraphics
        BBG.DrawImage(BB, 0, 0, Me.Width, Me.Height)

        'Clear BackOverdraw
        G.Clear(Color.Wheat)

    End Sub
    Private Sub TickCounter()

        ' Frame Counter 

        If tSec = TimeOfDay.Second And isRunning = True Then
            tTicks += 1
        Else
            MaxTicks = tTicks
            tTicks = 0
            tSec = TimeOfDay.Second
        End If
    End Sub

    Private Sub Form1_MouseClick(sender As Object, e As MouseEventArgs) Handles Me.MouseClick

        ' Previously, the code has painted two tiles red and blue. The red tile is located at (21,4) and the blue tile is at (21,6).
        ' When these tiles are clicked, the paint brush will be set to 1 or 2 allowing for users to 'color' and fill the grid tiles in with its respective color selected.


        If mouseX = 21 And mouseY = 4 Then
            paintBrush = 1
        ElseIf mouseX = 21 And mouseY = 6 Then
            paintBrush = 2
        End If

        'Click on tile, change fill color.
        Select Case paintBrush
            Case 0
            Case 1 'Red
                Map(mMapX, mMapY, 0) = 1
            Case 2 'Blue
                Map(mMapX, mMapY, 0) = 2
        End Select
    End Sub

    Private Sub Form1_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        'Divides the screen into a grid with 32/32 tiles.
        mouseX = Math.Floor(e.X / TileSize)
        mouseY = Math.Floor(e.Y / TileSize)

        ' Finds where the mouse is located relative to the grid.
        mMapX = MapX + mouseX
        mMapY = MapY + mouseY

    End Sub
End Class
