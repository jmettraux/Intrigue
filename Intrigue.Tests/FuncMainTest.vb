
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting


<TestClass()> Public Class FunctionsTest

    <TestMethod()> Public Sub Function_quote()

        Assert.AreEqual(
            "(1 2 3)",
            Interpreter.DoEval("(quote 1 2 3)").ToString)
        Assert.AreEqual(
            "(1 2 3)",
            Interpreter.DoEval("'(1 2 3)").ToString)
    End Sub

    <TestMethod()> Public Sub Function_if()

        Assert.AreEqual(
            "1",
            Interpreter.DoEval("if (= 7 7) 1 0").ToString)
        Assert.AreEqual(
            "0",
            Interpreter.DoEval("if (= 7 6) 1 0").ToString)
    End Sub

    <TestMethod()> Public Sub Function_if_when_missing_else()

        Assert.AreEqual(
            "1",
            Interpreter.DoEval("if (= 7 7) 1").ToString)
        Assert.AreEqual(
            "false",
            Interpreter.DoEval("if (= 7 6) 1").ToString)
    End Sub
End Class
