Imports System.Runtime.CompilerServices

Module ViewExtensions
    <Extension()>
    Public Function GetDpiScaleFactor(visual As Visual) As Point
        Dim source = PresentationSource.FromVisual(visual)
        If Not source Is Nothing AndAlso Not source.CompositionTarget Is Nothing Then
            Return New Point(
                source.CompositionTarget.TransformToDevice.M11,
                source.CompositionTarget.TransformToDevice.M22)
        End If
        Return New Point(1.0, 1.0)
    End Function

End Module
