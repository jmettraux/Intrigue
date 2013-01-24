
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
            """hello""",
            Interpreter.DoEval("""hello""").ToString)
        Assert.AreEqual(
            """hell o""",
            Interpreter.DoEval("""hell o""").ToString)
    End Sub

    <TestMethod()> Public Sub Interpreter_eval_boolean()

        Assert.AreEqual(
            "true",
            Interpreter.DoEval("true").ToString)
        Assert.AreEqual(
            "false",
            Interpreter.DoEval("false").ToString)
    End Sub

    <TestMethod()> Public Sub Interpreter_bind_lookup_atom()

        Dim i = New Intrigue.Interpreter

        i.BindAtom("a", 3)

        Assert.AreEqual("3", i.Lookup("a").ToString)
        Assert.AreEqual(3, i.LookupAtom("a"))
    End Sub
End Class
