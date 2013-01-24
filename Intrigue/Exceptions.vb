' 
' Copyright (c) 2013-2013, John Mettraux, jmettraux@gmail.com
'
' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in
' all copies or substantial portions of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
' THE SOFTWARE.
'
' Made in Japan
'

Imports Intrigue.Parsing


Namespace Ex

    Public Class IntrigueException
        Inherits Exception

        Public Sub New(message As String)

            MyBase.New(message)
        End Sub
    End Class

    Public Class NotFoundException
        Inherits IntrigueException

        Public Sub New(varname As String)

            MyBase.New("did not find a value for variable '" & varname & "'")
        End Sub
    End Class

    Public Class NotApplicableException
        Inherits IntrigueException

        Public Sub New(varname As String)

            MyBase.New("value held in '" & varname & "' is not applicable")
        End Sub
    End Class

    Public Class ArgException
        Inherits IntrigueException

        Public Sub New(message As String)

            MyBase.New(message)
        End Sub
    End Class

    Public Class UnbalancedParentheseException
        Inherits IntrigueException

        Public Sub New(t As Token)

            MyBase.New("unbalanced parenthese at " & t.Pos.ToLineAndColumn)
        End Sub
    End Class
End Namespace
