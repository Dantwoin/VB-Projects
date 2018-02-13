Imports System.IO

' Image Converter
' Created by Antwoin Davis 
' Desc: Designed to take a photo from the computer and turn it into a grayscale BMP. My original intent is to produce heightmaps using this program.

Public Class Form1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click
        ' Try: Prompt user for filename of new grayscale image.
        ' Catch: In the event that the picture is unable to be saved, the catch will throw a message containing the exception message.....
        Try
            Dim fileSaveBox As New SaveFileDialog

            'Saves images only in .BMP
            fileSaveBox.Filter = "BMP files (*.BMP)|*.bmp"
            fileSaveBox.FilterIndex = 1
            'If user changes directory focus to save new file, this flag will return the directory focus back to before the save occured. 
            fileSaveBox.RestoreDirectory = True


            If fileSaveBox.ShowDialog() = DialogResult.OK Then
                ConvertImage.Image.Save(fileSaveBox.FileName)
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)

        End Try



    End Sub

    Private Sub fileNameBox_TextChanged(sender As Object, e As EventArgs) Handles fileNameBox.TextChanged
        ' Take what is writen in the filenamebox and display that picture in the Preview Picture Box. Picture displayed is rescaled.
        PreviewImage.ImageLocation = fileNameBox.Text
        PreviewImage.SizeMode = PictureBoxSizeMode.Zoom
    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles OpenButton.Click
        Dim myFileBrowser As New OpenFileDialog

        'specifies what type of data files to look for
        myFileBrowser.Filter = "All Files (*.*)|*.*" & _
            "|Bitmaps (*.BMP)|*.bmp" & _
            "|Graphics Interchange Format (*.GIF)|*.gif" & _
            "|Joint Photographic Experts Group (*.JPEG)|*.jpeg" & _
            "|Joint Photographic Experts Group (*.JPG)|*.jpg" & _
            "|Exchangeable Image File  (*.EXIF)|*.EXIF" & _
            "|Portable Network Graphics (*.PNG)|*.png" & _
            "|Tag Image File Format (*.TIFF)|*.tiff"

        'specifies which data type is focused on start up
        myFileBrowser.FilterIndex = 1

        'Gets or sets a value indicating whether the dialog box restores the current directory before closing.
        myFileBrowser.RestoreDirectory = True

        'seperates message outputs for files found or not found
        If myFileBrowser.ShowDialog() = _
            DialogResult.OK Then
            If Dir(myFileBrowser.FileName) <> "" Then

            Else
                MsgBox("File Not Found", _
                       MsgBoxStyle.Critical)
            End If
        End If

        'Adds the file directory to the text box
        fileNameBox.Text = myFileBrowser.FileName



    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'read image
        Dim bmp As Bitmap = New Bitmap(fileNameBox.Text)

        'get image dimension
        Dim width As Integer = bmp.Width
        Dim height As Integer = bmp.Height

        'color of pixel

        Dim avg As Integer
        For x As Integer = 0 To width - 1
            For y As Integer = 0 To height - 1

        'get pixel value find average for each pixel
                avg = (CInt(bmp.GetPixel(x, y).R) + _
                       bmp.GetPixel(x, y).G + _
                       bmp.GetPixel(x, y).B) \ 3

                'set new pixel value
                bmp.SetPixel(x, y, Color.FromArgb(avg, avg, avg))

            Next y
        Next x


        'load grayscale image in ConvertImage Box
        ConvertImage.Image = bmp
        ConvertImage.SizeMode = PictureBoxSizeMode.Zoom
    End Sub
End Class
