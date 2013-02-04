
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting


<TestClass()> Public Class NumericFunctionsTest

    <TestMethod()> Public Sub Function_comparisons()

        Assert.AreEqual(
            "true",
            Interpreter.DoEval("(< 2 3)").ToString)
        Assert.AreEqual(
            "false",
            Interpreter.DoEval("(> 2 3)").ToString)

        Assert.AreEqual(
            "false",
            Interpreter.DoEval("> 1.2 1.3").ToString)
        Assert.AreEqual(
            "true",
            Interpreter.DoEval("<= 1.3 1.3").ToString)
        Assert.AreEqual(
            "true",
            Interpreter.DoEval("<= 1.0 1").ToString)
        Assert.AreEqual(
            "true",
            Interpreter.DoEval("< 1.2 1.3").ToString)
        Assert.AreEqual(
            "true",
            Interpreter.DoEval("< 1.2 2").ToString)

        Assert.AreEqual(
            "true",
            Interpreter.DoEval("(<= 3 3)").ToString)
        Assert.AreEqual(
            "true",
            Interpreter.DoEval("(>= 3 3)").ToString)

        Assert.AreEqual(
            "true",
            Interpreter.DoEval("(= 3 3)").ToString)
        Assert.AreEqual(
            "false",
            Interpreter.DoEval("(= 3 2)").ToString)
    End Sub

    <TestMethod()> Public Sub Function_plus()

        Assert.AreEqual(
            "5",
            Interpreter.DoEval("(+ 2 3)").ToString)
        Assert.AreEqual(
            "5",
            Interpreter.DoEval("+ 2 3").ToString)
        Assert.AreEqual(
            "0",
            Interpreter.DoEval("(+)").ToString)
        Assert.AreEqual(
            "2.5",
            Interpreter.DoEval("+ 2 .5").ToString)
    End Sub

    <TestMethod()> Public Sub Function_minus()

        Assert.AreEqual(
            "5",
            Interpreter.DoEval("- 8 3").ToString)
        Assert.AreEqual(
            "5.0",
            Interpreter.DoEval("- 8 3.0").ToString)

        Dim ex As Exception = Nothing
        Try
            Interpreter.DoEval("- 1 2 3")
            Assert.IsTrue(False)
        Catch ex
        End Try
        Assert.AreEqual("Intrigue.Ex.ArgException", ex.GetType.ToString)
        Assert.AreEqual("'-' expects 1 to 2 arguments, not 3", ex.Message)
    End Sub

    <TestMethod()> Public Sub Function_star()

        Assert.AreEqual(
            "-24",
            Interpreter.DoEval("* 8 3 -1").ToString)
        Assert.AreEqual(
            "1",
            Interpreter.DoEval("(*)").ToString)

        Assert.AreEqual(
            "2.4",
            Interpreter.DoEval("* 2 1.2").ToString)
    End Sub

    <TestMethod()> Public Sub Function_slash()

        Assert.AreEqual(
            "-2",
            Interpreter.DoEval("/ 8 -4").ToString)
        Assert.AreEqual(
            "0",
            Interpreter.DoEval("/ 2").ToString)
        Assert.AreEqual(
            "0.5",
            Interpreter.DoEval("/ 2.0").ToString)

        Dim ex As Exception = Nothing
        Try
            Interpreter.DoEval("(/)")
            Assert.IsTrue(False)
        Catch ex
        End Try
        Assert.AreEqual("Intrigue.Ex.ArgException", ex.GetType.ToString)
        Assert.AreEqual("'/' expects 1 to 2 arguments, not 0", ex.Message)
    End Sub
End Class
