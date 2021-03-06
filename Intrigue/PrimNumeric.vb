﻿' 
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

    Public Class PlusPrimitive
        Inherits PrimitiveNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            Dim isum = 0
            Dim dsum = 0.0
            Dim d = False

            For Each n In args.Nodes
                Dim v = env.Eval(n)
                If v.IsInexact Then d = True
                isum += v.ToInteger
                dsum += v.ToDouble
            Next

            Return New AtomNode(IIf(d, dsum, isum))
        End Function
    End Class

    Public Class MinusPrimitive
        Inherits PrimitiveNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 1, 2)

            Dim a = env.Eval(args.Car)
            Dim isum = a.ToInteger
            Dim dsum = a.ToDouble
            Dim d = a.IsInexact

            For Each n As Node In args.Cdr.Nodes
                Dim v = env.Eval(n)
                If v.IsInexact Then d = True
                isum -= v.ToInteger
                dsum -= v.ToDouble
            Next

            Return New AtomNode(IIf(d, dsum, isum))
        End Function
    End Class

    Public Class StarPrimitive
        Inherits PrimitiveNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            Dim ipro = 1
            Dim dpro = 1.0
            Dim d = False

            For Each n In args.Nodes
                Dim v = env.Eval(n)
                If v.IsInexact Then d = True
                ipro *= v.ToInteger
                dpro *= v.ToDouble
            Next

            Return New AtomNode(IIf(d, dpro, ipro))
        End Function
    End Class

    Public Class SlashPrimitive
        Inherits PrimitiveNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 1, 2)

            Dim a = env.Eval(args.Car)

            If args.Length > 1 Then

                Dim b = env.Eval(args.Cdr.Car)

                If a.IsInexact OrElse b.IsInexact Then
                    Return New AtomNode(a.ToDouble / b.ToDouble)
                Else
                    Return New AtomNode(Convert.ToInt64(a.ToInteger / b.ToInteger))
                End If
            End If

            If a.IsInexact Then
                Return New AtomNode(1.0 / a.ToDouble)
            Else
                Return New AtomNode(Convert.ToInt64(1 / a.ToInteger))
            End If
        End Function
    End Class

    Public Class GreaterPrimitive
        Inherits PrimitiveNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 2)

            Dim na = env.Eval(args.Nodes(0)).ToAtomNode
            Dim nb = env.Eval(args.Nodes(1)).ToAtomNode

            If na.IsInexact OrElse nb.IsInexact Then

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

    Public Class MaxMinPrimitive
        Inherits PrimitiveNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            Dim max As Double = 0.0
            Dim min As Double = 0.0
            Dim inexact = False

            For Each n In args.Nodes

                Dim v = env.Eval(n)
                Dim dv = v.ToDouble()

                If v.IsInexact Then inexact = True

                If dv > max Then
                    max = dv
                ElseIf dv < min Then
                    min = dv
                End If
            Next

            Dim r As Object = max
            If funcName = "min" Then r = min
            If inexact = False Then r = Convert.ToInt64(r)

            Return New AtomNode(r)
        End Function
    End Class

    Public Class AbsPrimitive
        Inherits PrimitiveNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 1)

            Return New AtomNode(Math.Abs(env.Eval(args.Car).ToAtomNode.Atom))
        End Function
    End Class

    Public Class QrmPrimitive
        Inherits PrimitiveNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 2)

            Dim a = env.Eval(args.Car)
            Dim b = env.Eval(args.Cdr.Car)

            If Not a.IsNumber OrElse Not b.IsNumber Then
                Throw New Ex.ArgException("'" & funcName & "' expects numbers")
            End If

            Dim ia = a.ToAtomNode.Atom
            Dim ib = b.ToAtomNode.Atom

            Dim ir As Long = 0

            If funcName = "quotient" Then
                ir = Convert.ToInt64(ia / ib)
            ElseIf funcName = "remainder" Then
                Math.DivRem(ia, ib, ir)
            Else ' modulo
                ir = ia Mod ib
            End If

            If a.IsInexact OrElse b.IsInexact Then
                Return New AtomNode(Convert.ToDouble(ir))
            Else
                Return New AtomNode(ir)
            End If
        End Function
    End Class

    Public Class CheckExactPrimitive
        Inherits PrimitiveNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 1)

            Dim val = env.Eval(args.Car)

            If funcName = "exact?" Then
                Return New AtomNode(val.IsExact)
            Else ' inexact?
                Return New AtomNode(val.IsInexact)
            End If
        End Function
    End Class

    Public Class CheckSignPrimitive
        Inherits PrimitiveNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 1)

            Dim val = env.Eval(args.Car)

            If Not val.IsAtom Then Return New AtomNode(False)

            Dim ival = val.ToInteger

            If funcName = "zero?" Then
                Return New AtomNode(ival = 0)
            ElseIf funcName = "positive?" Then
                Return New AtomNode(ival > 0)
            Else ' negative?
                Return New AtomNode(ival < 0)
            End If
        End Function
    End Class

    Public Class CheckOddnessPrimitive
        Inherits PrimitiveNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Environment) As Node

            CheckArgCount(funcName, args, 1)

            Dim val = env.Eval(args.Car)

            If Not val.IsExact Then Return New AtomNode(False)

            Dim m = val.ToInteger Mod 2

            If funcName = "odd?" Then
                Return New AtomNode(m <> 0)
            Else ' even?
                Return New AtomNode(m = 0)
            End If
        End Function
    End Class
End Namespace
