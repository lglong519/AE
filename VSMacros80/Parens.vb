Option Strict Off
Option Explicit Off
Imports System
Imports EnvDTE
Imports EnvDTE80
Imports EnvDTE90
Imports EnvDTE90a
Imports EnvDTE100
Imports System.Diagnostics

Public Module Parens

    Sub TemporaryMacro()
        DTE.ActiveDocument.Selection.Text = "("
        DTE.ActiveDocument.Selection.Text = ")"
        DTE.ActiveDocument.Selection.CharLeft()
    End Sub
End Module
