
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting


<TestClass()> Public Class LibAlistsTest

    <TestMethod()> Public Sub LibFunc_alist()

        Assert.AreEqual(
            "false",
            Interpreter.DoEval("alist? true").ToString)
        Assert.AreEqual(
            "true",
            Interpreter.DoEval("alist? '()").ToString)
        Assert.AreEqual(
            "false",
            Interpreter.DoEval("alist? '(1 2 3)").ToString)
        Assert.AreEqual(
            "true",
            Interpreter.DoEval("alist? '((1 2) (3 4))").ToString)
        Assert.AreEqual(
            "false",
            Interpreter.DoEval("alist? '((1 2) ())").ToString)
    End Sub

    <TestMethod()> Public Sub LibFunc_assoc()

        Assert.AreEqual(
            "false",
            Interpreter.DoEval("assoc 'd '((a 0) (b 1) (c 2))").ToString)

        Assert.AreEqual(
            "(a 0)",
            Interpreter.DoEval("assoc 'a '((a 0) (b 1) (c 2))").ToString)
        Assert.AreEqual(
            "(c 2)",
            Interpreter.DoEval("assoc 'c '((a 0) (b 1) (c 2))").ToString)
    End Sub
End Class
