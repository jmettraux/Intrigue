
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting


<TestClass()> Public Class FunctionTest

    <TestMethod()> Public Sub Function_quote()

        Assert.AreEqual(
            "(1 2 3)",
            Interpreter.DoEval("(quote 1 2 3)").ToString)
        Assert.AreEqual(
            "(1 2 3)",
            Interpreter.DoEval("'(1 2 3)").ToString)
    End Sub

    <TestMethod()> Public Sub Function_car()

        Assert.AreEqual(
            "1",
            Interpreter.DoEval("car '(1 2 3)").ToString)
        Assert.AreEqual(
            "1",
            Interpreter.DoEval("(car (quote 1 2 3))").ToString)
    End Sub

    <TestMethod()> Public Sub Function_cdr()

        Assert.AreEqual(
            "(2 3)",
            Interpreter.DoEval("cdr '(1 2 3)").ToString)
        Assert.AreEqual(
            "(2 3)",
            Interpreter.DoEval("(cdr (quote 1 2 3))").ToString)
    End Sub

    <TestMethod()> Public Sub Function_plus()

        Assert.AreEqual(
            "5",
            Interpreter.DoEval("(+ 2 3)").ToString)
        Assert.AreEqual(
            "5",
            Interpreter.DoEval("+ 2 3").ToString)
    End Sub
End Class
