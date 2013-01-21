
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting


<TestClass()> Public Class DefineTest

    <TestMethod()> Public Sub Define_bind_var()

        Dim i = New Intrigue.Interpreter

        i.Eval("(define a 10)")

        Assert.AreEqual("10", i.Lookup("a").ToString)
    End Sub

    <TestMethod()> Public Sub Define_bind_var_2()

        Assert.AreEqual(
            "12",
            Intrigue.Interpreter.DoEval(
                <string>
                    define a 12
                    a
                </string>.Nodes.First.ToString.Trim
            ).ToString)

        Assert.AreEqual(
            "12",
            Intrigue.Interpreter.DoEval(New String() {
                "define a 12",
                "a"
            }).ToString)
    End Sub

    <TestMethod()> Public Sub Define_function()

        Assert.AreEqual(
            "14",
            Intrigue.Interpreter.DoEval(
                <string>
                    define (double x) (+ x x)
                    double 7
                </string>.Nodes.First.ToString.Trim
            ).ToString)
    End Sub
End Class
