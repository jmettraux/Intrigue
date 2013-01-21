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


Public Class QuoteFunctionNode
    Inherits FunctionNode

    Public Overrides Function Apply(ByRef args As ListNode, ByRef context As Context) As Node

        Return args
    End Function
End Class

Public Class CarFunctionNode
    Inherits FunctionNode

    Public Overrides Function Apply(ByRef args As ListNode, ByRef context As Context) As Node

        If args.Length <> 1 Then
            Throw New ArgException("'car' expects 1 argument, not " & args.Length)
        End If

        Return context.Eval(args.Car).Car
    End Function
End Class

Public Class CdrFunctionNode
    Inherits FunctionNode

    Public Overrides Function Apply(ByRef args As ListNode, ByRef context As Context) As Node

        If args.Length <> 1 Then
            Throw New ArgException("'cdr' expects 1 argument, not " & args.Length)
        End If

        Return context.Eval(args.Car).Cdr
    End Function
End Class

Public Class ConsFunctionNode
    Inherits FunctionNode

    Public Overrides Function Apply(ByRef args As ListNode, ByRef context As Context) As Node

        If args.Length <> 2 Then
            Throw New ArgException("'cons' expects 2 arguments, not " & args.Length)
        End If

        Dim head = context.Eval(args.Car)
        Dim tail = context.Eval(args.Cdr.Car)

        If tail.IsList Then
            tail.toListNode.Nodes.Insert(0, head)
            Return tail
        Else
            Return New ListNode(head, tail)
        End If
    End Function
End Class

Public Class PlusFunctionNode
    Inherits FunctionNode

    Public Overrides Function Apply(ByRef args As ListNode, ByRef context As Context) As Node

        Dim i = 0
        Dim v As AtomNode

        For Each n In args.Nodes

            v = TryCast(context.Eval(n), AtomNode)

            If v Is Nothing Then
                Throw New ArgException("'+' cannot add lists")
            End If

            i = i + Convert.ToInt64(v.Atom)
        Next

        Return New AtomNode(i)
    End Function
End Class

Public Class DefineFunctionNode
    Inherits FunctionNode

    Public Overrides Function Apply(ByRef args As ListNode, ByRef context As Context) As Node

        If args.Length <> 2 Then
            Throw New ArgException("'define' expects 2 arguments, not " & args.Length)
        End If

        Dim car = args.Car

        Dim name As String = Nothing
        Dim value As Node = Nothing

        If car.IsList Then
            name = TryCast(car.Car.ToAtomNode.Atom, String)
            value = New LambdaNode(New ListNode(car.Cdr, args.Cdr.Car))
        ElseIf car.IsSymbol Then
            name = car.ToString
        ElseIf car.IsAtom Then
            name = TryCast(car.ToAtomNode.Atom, String)
        End If

        If name Is Nothing Then
            Throw New ArgException("'define' expects a Symbol/String as first argument")
        End If

        If value Is Nothing Then
            value = context.Eval(args.Cdr.Car)
        End If

        context.Bind(name, value)

        Return New AtomNode(name)
    End Function
End Class

Public Class LambdaNode
    Inherits FunctionNode

    Protected definition As ListNode

    Public Sub New(ByRef definition As ListNode)

        Me.definition = definition
    End Sub

    Public Overrides Function Apply(ByRef args As ListNode, ByRef context As Context) As Node

        Console.WriteLine("def: " & Me.definition.ToString)
        Console.WriteLine("args: " & args.ToString)

        Dim c = New Context(context)

        Dim argnames = Me.definition.Car.toListNode.Nodes
        Dim body = Me.definition.Cdr.Car

        For i = 0 To argnames.Count - 1

            Dim argname = argnames(i).ToString
            Dim value = context.Eval(args.Nodes(i))

            c.Bind(argname, value)
        Next

        Return c.Eval(body)
    End Function
End Class

Public Class LambdaFunctionNode
    Inherits FunctionNode

    Public Overrides Function Apply(ByRef args As ListNode, ByRef context As Context) As Node

        Return New LambdaNode(args)
    End Function
End Class
