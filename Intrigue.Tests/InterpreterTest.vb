
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting


<TestClass()> Public Class InterpreterTest

    <TestMethod()> Public Sub Interpreter_eval_integer()

        Assert.AreEqual(
            "12",
            Interpreter.DoEval("12").ToString)
    End Sub

    <TestMethod()> Public Sub Interpreter_eval_string()

        Assert.AreEqual(
            "hello",
            Interpreter.DoEval("hello").ToString)
        Assert.AreEqual(
            """hell o""",
            Interpreter.DoEval("""hell o""").ToString)
    End Sub
End Class
