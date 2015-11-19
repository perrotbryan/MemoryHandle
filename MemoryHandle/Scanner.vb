Public Module Scanner
    Private Function Scan(ByVal pid As Integer, ByVal pstartaddr As IntPtr, ByVal ppattern() As Byte, ByVal pmask As String, ByVal pbuffersize As Integer) As Byte()
        Dim buffer(pbuffersize - 1) As Byte
        Dim bytesread As Integer = 0

        Dim hProcess As IntPtr = OpenProcess(ProcessAccessFlags.All, True, pid)
        Dim readsuccess As Boolean = ReadProcessMemory(hProcess, pstartaddr, buffer, pbuffersize, bytesread)

        While readsuccess AndAlso bytesread = pbuffersize
            Dim pos As Integer = Find(buffer, ppattern, pmask)
            If pos <> -1 Then
                CloseHandle(hProcess)
                Return SubArray(buffer, pos, ppattern.Length)
            End If

            pstartaddr += (pbuffersize - ppattern.Length - 1)
            readsuccess = ReadProcessMemory(hProcess, pstartaddr, buffer, pbuffersize, bytesread)
        End While

        CloseHandle(hProcess)
        Return Nothing
    End Function

    Public Function Scan(ByVal pid As Integer, ByVal pstartAddr As IntPtr, ByVal ppattern As String, ByVal pmask As String, ByVal pbufferSize As Integer) As Byte()
        ppattern = ppattern.Replace("x", "&H")
        Dim str() As String = ppattern.Split("\")
        Dim len As Integer = str.Length - 1
        Dim bytearray(len) As Byte
        For i = 0 To len
            bytearray(i) = CByte(str(i))
        Next
        Return Scan(pid, pstartAddr, bytearray, pmask, pbufferSize)
    End Function

    ''' <summary>
    ''' Searches a pattern (array of bytes) inside the buffer (array of bytes)
    ''' </summary>
    ''' <param name="buffer"></param>
    ''' <param name="pattern"></param>
    ''' <param name="mask"></param>
    ''' <returns>Returns the position found</returns>
    ''' <remarks></remarks>
    Private Function Find(ByVal buffer As Byte(), ByVal pattern As Byte(), ByVal mask As String) As Integer
        Dim patlen As Integer = pattern.Length
        Dim buflen As Integer = buffer.Length - patlen
        For i = 0 To buflen

            Dim j As Integer
            For j = 0 To patlen - 1
                If mask(j) = "?" OrElse mask(j) = "!" Then Continue For
                If buffer(i + j) <> pattern(j) Then
                    Exit For
                End If
            Next
            If j = patlen Then
                Return i
            End If
        Next
        Return -1
    End Function

    Private Function SubArray(ByVal haystack() As Byte, ByVal start As Integer, ByVal length As Integer) As Byte()
        Dim ARETOURNER(length - 1) As Byte
        Array.Copy(haystack, start, ARETOURNER, 0, length)
        Return ARETOURNER
    End Function

    Public Function FindOffset(ByVal pid As Integer, ByVal pstartaddr As IntPtr, ByVal ppattern() As Byte, ByVal pmask As String, ByVal pbuffersize As Integer) As IntPtr
        If pbuffersize <= ppattern.Length OrElse ppattern.Length <> pmask.Length Then Return IntPtr.Zero
        Dim res() As Byte = Scan(pid, pstartaddr, ppattern, pmask, pbuffersize)
        Dim val(pmask.Split("?").Length - 1) As Byte
        If res Is Nothing Then Return IntPtr.Zero
        For i = 0 To res.Length - 1
            If pmask(i) = "?" Then val(Array.IndexOf(val, Nothing)) = res(i)
        Next
        Return New IntPtr(BitConverter.ToInt32(val, 0))
    End Function

    Public Function FindOffset(ByVal pid As Integer, ByVal pstartAddr As IntPtr, ByVal ppattern As String, ByVal pmask As String, ByVal pbufferSize As Integer) As IntPtr
        ppattern = ppattern.Replace("x", "&H")
        Dim str() As String = ppattern.Split("\")
        Dim len As Integer = str.Length - 1
        Dim bytearray(len) As Byte
        For i = 0 To len
            bytearray(i) = CByte(str(i))
        Next
        Return FindOffset(pid, pstartAddr, bytearray, pmask, pbufferSize)
    End Function
End Module
