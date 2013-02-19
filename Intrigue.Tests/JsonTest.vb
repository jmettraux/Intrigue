
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting


<TestClass()> Public Class JsonTest

    <TestMethod()> Public Sub Json_array()

        Assert.AreEqual(
            "(1 2 3 4)",
            Interpreter.DoEval("[ 1, 2, 3, 4 ]").ToString)
    End Sub

    <TestMethod()> Public Sub Json_object()

        Assert.AreEqual(
            "(33 ""Kiyomori"")",
            Interpreter.DoEval(Util.NewString(
                <![CDATA[
                    define user { "name": "Kiyomori", "age": 33 }
                    (list (user 'get "age") (user 'get "name"))
                ]]>
            )).ToString)
    End Sub

    <TestMethod()> Public Sub Json_object_alist()

        Assert.AreEqual(
            "((""name"" ""Kiyomori"") (""age"" 33))",
            Interpreter.DoEval(Util.NewString(
                <![CDATA[
                    define user { "name": "Kiyomori", "age": 33 }
                    user 'alist
                ]]>
            )).ToString)
    End Sub

    <TestMethod()> Public Sub Json_object_set()

        Assert.AreEqual(
            "((""age"" 55) (""name"" ""Kiyomori"") (""age"" 33))",
            Interpreter.DoEval(Util.NewString(
                <![CDATA[
                    define user { "name": "Kiyomori", "age": 33 }
                    user 'set "age" 55
                    user 'alist
                ]]>
            )).ToString)

        Assert.AreEqual(
            "55",
            Interpreter.DoEval(Util.NewString(
                <![CDATA[
                    define user { "name": "Kiyomori", "age": 33 }
                    user 'set "age" 55
                    user 'get "age"
                ]]>
            )).ToString)
    End Sub
End Class
