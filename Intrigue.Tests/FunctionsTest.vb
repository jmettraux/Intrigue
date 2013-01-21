
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
End Class
