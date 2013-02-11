
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
        Assert.AreEqual(
            "true",
            Interpreter.DoEval("#t").ToString)
        Assert.AreEqual(
            "false",
            Interpreter.DoEval("#f").ToString)
    End Sub

    <TestMethod()> Public Sub Interpreter_bind_lookup_atom()

        Dim i = New Intrigue.Interpreter

        i.BindAtom("a", 3)

        Assert.AreEqual("3", i.Lookup("a").ToString)
        Assert.AreEqual(3, i.LookupAtom("a"))
    End Sub

    <TestMethod()> Public Sub Interpreter_eval_negative_integers()

        Assert.AreEqual(
            "-13",
            Interpreter.DoEval("-13").ToString)
        Assert.AreEqual(
            "-13",
            Interpreter.DoEval("+ 10 -23").ToString)

        Assert.AreEqual(
            "-13",
            Interpreter.DoEval("+ -10 -3").ToString)
    End Sub

    <TestMethod()> Public Sub Interpreter_eval_floats()

        Assert.AreEqual(
            "1.2",
            Interpreter.DoEval("1.2").ToString)
        Assert.AreEqual(
            "-1.2",
            Interpreter.DoEval("-1.2").ToString)
        Assert.AreEqual(
            "0.5",
            Interpreter.DoEval(".5").ToString)
        Assert.AreEqual(
            "-0.5",
            Interpreter.DoEval("-.5").ToString)
    End Sub

    <TestMethod()> Public Sub Interpreter_eval_lambdas()

        Assert.AreEqual(
            "4",
            Intrigue.Interpreter.DoEval("(lambda (x) (+ x 1)) 3").ToString)
    End Sub
End Class
