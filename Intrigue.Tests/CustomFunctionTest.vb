
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports Intrigue
Imports Intrigue.Nodes


<TestClass()> Public Class CustomFunctionTest

    Class UpcaseFunction
        Inherits Intrigue.Nodes.FunctionNode

        Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef context As Context) As Node

            ' will raise if there isn't 1 and only 1 argument

            CheckArgCount(funcName, args, 1)

            ' eval argument and then extract value

            Dim s0 = DirectCast(context.Eval(args.Car).ToAtomNode.Atom, String)

            ' call ToUpper on argument string

            Dim s1 = s0.ToUpper

            Return New AtomNode(s1)
        End Function
    End Class

    <TestMethod()> Public Sub Custom_function()

        Dim i = New Intrigue.Interpreter
        i.Bind("upcase", New UpcaseFunction)

        Assert.AreEqual("""LONDON""", i.Eval("(upcase ""lonDon""").ToString)
    End Sub
End Class
