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

Namespace Nodes

    Public Class PlusFunctionNode
        Inherits FunctionNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            Dim isum = 0
            Dim dsum = 0.0
            Dim d = False

            For Each n In args.Nodes
                Dim v = env.Eval(n)
                If v.IsDouble Then d = True
                isum += v.ToInteger
                dsum += v.ToDouble
            Next

            Return New AtomNode(IIf(d, dsum, isum))
        End Function
    End Class

    Public Class MinusFunctionNode
        Inherits FunctionNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 1, 2)

            Dim a = env.Eval(args.Car)
            Dim isum = a.ToInteger
            Dim dsum = a.ToDouble
            Dim d = a.IsDouble

            For Each n As Node In args.Cdr.Nodes
                Dim v = env.Eval(n)
                If v.IsDouble Then d = True
                isum -= v.ToInteger
                dsum -= v.ToDouble
            Next

            Return New AtomNode(IIf(d, dsum, isum))
        End Function
    End Class

    Public Class StarFunctionNode
        Inherits FunctionNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            Dim ipro = 1
            Dim dpro = 1.0
            Dim d = False

            For Each n In args.Nodes
                Dim v = env.Eval(n)
                If v.IsDouble Then d = True
                ipro *= v.ToInteger
                dpro *= v.ToDouble
            Next

            Return New AtomNode(IIf(d, dpro, ipro))
        End Function
    End Class

    Public Class SlashFunctionNode
        Inherits FunctionNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 1, 2)

            Dim a = env.Eval(args.Car)

            If args.Length > 1 Then

                Dim b = env.Eval(args.Cdr.Car)

                If a.IsDouble OrElse b.IsDouble Then
                    Return New AtomNode(a.ToDouble / b.ToDouble)
                Else
                    Return New AtomNode(Convert.ToInt64(a.ToInteger / b.ToInteger))
                End If
            End If

            If a.IsDouble Then
                Return New AtomNode(1.0 / a.ToDouble)
            Else
                Return New AtomNode(Convert.ToInt64(1 / a.ToInteger))
            End If
        End Function
    End Class

    Public Class GreaterFunctionNode
        Inherits FunctionNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 2)

            Dim na = env.Eval(args.Nodes(0)).ToAtomNode
            Dim nb = env.Eval(args.Nodes(1)).ToAtomNode

            If na.IsDouble OrElse nb.IsDouble Then

                Dim da = na.ToDouble
                Dim db = nb.ToDouble

                If funcName.EndsWith("=") AndAlso da = db Then Return New AtomNode(True)
                If funcName = "=" Then Return New AtomNode(False)

                If funcName.StartsWith("<") Then
                    Return New AtomNode(da < db)
                Else
                    Return New AtomNode(da > db)
                End If

            Else

                Dim ia = na.ToInteger
                Dim ib = nb.ToInteger

                If funcName.EndsWith("=") AndAlso ia = ib Then Return New AtomNode(True)
                If funcName = "=" Then Return New AtomNode(False)

                If funcName.StartsWith("<") Then
                    Return New AtomNode(ia < ib)
                Else
                    Return New AtomNode(ia > ib)
                End If
            End If
        End Function
    End Class

    Public Class CheckExactFunctionNode
        Inherits FunctionNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 1)

            Dim val = env.Eval(args.Car)

            If Not val.IsAtom Then Return New AtomNode(False)

            Dim type = val.ToAtomNode.Atom.GetType.ToString.Split(".").Last.ToLower

            If funcName = "exact?" Then
                If type = "integer" Or type = "int32" Or type = "int64" Then Return New AtomNode(True)
            Else ' inexact?
                If type = "float" Or type = "double" Then Return New AtomNode(True)
            End If

            Return New AtomNode(False)
        End Function
    End Class
End Namespace
