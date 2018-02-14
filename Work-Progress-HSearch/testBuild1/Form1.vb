Imports System.Drawing
Imports System.Threading.Tasks

' Antwoin Davis
' Code: Helicopter Search like Ants
' Description: Helicopter will search for objects. This is AI practice program. 
'
'  Current Features: Helicopters fly around within boundaries.  Display Debuging Stats. Exits when Esc is pressed
'




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
    Dim debug As Boolean
    Dim tSec As Integer = TimeOfDay.Second
    Dim tTicks As Integer = 0
    Dim MaxTicks As Integer = 0

    'Map Variables
    Dim Map(,) As Integer

    ' Paint Brush Variabe
    Dim paintBrush As Integer

    'GAME RUNNING
    Dim isRunning As Boolean = True

    Dim mouseX As Integer
    Dim mouseY As Integer
    Dim mMapX As Integer
    Dim mMapY As Integer

    Dim heli(10) As Helicopter

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            isRunning = False
        End If
    End Sub


    Private Sub Form1_load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Show()
        Me.Focus()

        'Initialize Graphic Objects

        G = Me.CreateGraphics
        BB = New Bitmap(Me.Width, Me.Height)

        ' Initialize Map variables
        Me.Height = 450
        Me.Width = 750

        ReDim Map(Me.Width - 1, Me.Height - 1)
        For k = 0 To Me.Width - 1
            For j = 0 To Me.Height - 1
                Map(k, j) = 0
            Next
        Next
        'Initialize Helicopters : Me.width and me. Width are off by 11 and 38 respectively. 

        Dim rand As Random = New Random()
        For i = 0 To 10
            heli(i) = New Helicopter(650, 100, rand.Next(0, Me.Width), rand.Next(0, Me.Height), Me.Width - 11, Me.Height - 38)
        Next

        'Game Loop begins
        GameController()

        'When GameController ends. The program will close itself.
        Me.Close()

    End Sub

    Private Sub GameController()
        Do While isRunning = True
            Application.DoEvents()

            ' Check User Input
            ' Run AI
            HeliUpdate()
            ' Update Obj Data
            TickCounter()
            ' Check Triggers and Conditions
            ' Draw Graphics
            DrawGraphics()
        Loop
    End Sub

    Private Sub HeliUpdate()
        'look around
        'move
        For i = 0 To 10
            heli(i).move()
        Next

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

    Private Sub DrawGraphics()
        'Write to BackBuffer

        G.DrawString("Ticks: " & tTicks & vbCrLf & _
                      "FPS: " & MaxTicks & vbCrLf & _
                      "Mouse X: " & mouseX & vbCrLf & _
                      "Mouse Y: " & mouseY & vbCrLf & _
                      "Map Data: " & Map(mouseX, mouseY), _
                        Me.Font, Brushes.Black, 650, 0)
        G.DrawString("Press Esc to exit.", Me.Font, Brushes.Black, 300, 0)

        'Draw Helicopters
        For i = 0 To 10
            heli(i).draw(G)
        Next


        'Copy BackBuffer to Graphics Object
        G = Graphics.FromImage(BB)

        'Draw Backbuffer to Screen
        BBG = Me.CreateGraphics
        BBG.DrawImage(BB, 0, 0, Me.Width, Me.Height)

        'Clear BackOverdraw
        G.Clear(Color.Wheat)

    End Sub

    Private Sub Form1_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        'Enform boundaries. Since debugger will check these values, the values can not return anthing out of the scope of the 2d Map array.
        If e.X > 0 And e.X < Me.Width Then mouseX = e.X
        If e.Y > 0 And e.Y < Me.Height Then mouseY = e.Y
    End Sub
End Class

Public Class Helicopter

    Dim src As Point
    Dim des As Point
    Dim width As UShort = 9
    Dim height As UShort = 9
    Dim xbound As Integer
    Dim ybound As Integer
    Dim bmp As New Drawing.Bitmap(width, height)
    Dim speed As Double = 1
    Dim g As Graphics
    Dim pb As System.Drawing.Pen


    Sub New(ByVal srcx, ByVal srcy, ByVal desx, ByVal desy, ByVal bx, ByVal by)
        'Pick a random point to begin search
        src = New Point(srcx, srcy)
        des = New Point(desx, desy)

        'set bounderies helicopter to fly in
        xbound = bx
        ybound = by

        'Image for heli to be printed to the screen
        bmp = Image.FromFile("heli.png")
        pb = New System.Drawing.Pen(Color.Blue, 1)
    End Sub

    Sub newDes()
        Dim rand As Random = New Random
        Dim t As Integer = 100
        Dim posx As Integer
        Dim posy As Integer
        Do
            posx = des.X
            posy = des.Y
            posx += rand.Next(-t, t)
            posy += rand.Next(-t, t)
        Loop Until (posx < xbound) And (posy < ybound) And (posx > 0) And (posy > 0)
        des = New Point(posx, posy)
    End Sub

    Sub draw(ByRef g As Graphics)
        g.DrawImage(bmp, src.X, src.Y)
    End Sub
    Sub move()
        ' Helicopter selects a destination to fly in
        If (src.X = des.X) And (src.Y = des.Y) Then
            newDes()
        Else

            Dim posX As Integer = src.X
            Dim posY As Integer = src.Y
            ' Check if direction is blocked
            'checknext()

            ' move 
            Dim xDir As Integer = des.X - src.X
            Dim yDir As Integer = des.Y - src.Y
            Dim hyp As Integer = Math.Sqrt(xDir * xDir + yDir * yDir)

            xDir = xDir / hyp
            yDir = yDir / hyp

            posX = posX + xDir * speed
            posY = posY + yDir * speed

            src = New Point(posX, posY)
        End If

    End Sub
    Sub checknext(ByRef x, ByRef y)

    End Sub
End Class
