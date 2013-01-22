
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports Intrigue


<TestClass()> Public Class TokenizerTest

    <TestMethod()> Public Sub Tokenizer_Tokenize()

        Dim ts = Intrigue.Parsing.Tokenizer.Tokenize(
            <string>
               (alpha 1 2 3) # nada
               # nada
               bravo 4 5 6 # nada
               "hello"
           </string>.Nodes.First.ToString
        )
        For Each t In ts
            Console.WriteLine(t.ToString)
        Next

        Assert.AreEqual(26, ts.Count)
    End Sub
End Class
