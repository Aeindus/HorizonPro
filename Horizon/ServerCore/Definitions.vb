Public Module Definitions
    Public Enum ComponentTypes
        shell = 0
        screenCapture = 1
        ftp = 2
        hooks = 3
        autodestroy = 4
        audio = 5
        bsod = 6
        upload_manager = 7
        download_manager = 8
        regedit = 10
        mouse = 11
        any_load = 12
    End Enum

    Public Enum DataTypes
        ping_send = 1
        ping_acknowledged = 2
        close_client = 3
        authentificate = 4
        abort = 5

        '' Basic tcp signaling messages
        tcp_error_debugging = 6
        com_error_debugging = 7
        com_notification = 8

        load_component = 9
        free_component = 10     ' Calls FreeLibraryAndExitThread on client side

        '' Used for synchronising network buffers and for proper component cancellation
        flags_cancel_operation = 11
        flags_cancelled_operation = 12

        '' Chunked messages
        start_chunked_operation = 13
        stop_chunked_operation = 14
        process_chunk_operation = 15

        '' SHELL COMMANDS
        shell_init = 16
        shell = 17

        '' FTP TRANSACTION COMMANDS 
        enum_drives = 18
        enum_folders_and_files = 19
        download_file = 20
        download_folder = 21
        download_folder_end = 22
        delete_folder = 23
        delete_file = 24
        rename = 25
        create_folder = 26
        create_file = 27
        upload_file = 28
        run = 29

        '' REGEDIT TRANSASCTION
        enum_keys = 30
        enum_values = 31
        rename_key = 32
        delete_key = 33
        hide_key = 34
        create_key = 35
        create_value = 36

        '' VIDEO TRANSASCTION
        video_init = 37
        video_frame = 38
        video_start = 39
        video_end = 40

        '' MOUSE TRANSASCTION
        mouse_start = 41
        mouse_pos = 42
        mouse_click_left = 43
        mouse_click_right = 44
    End Enum

    '' Used for most validation operations needing only a string
    Public Structure ValidationResponse
        Public Successful As Boolean
        Public Information As String
    End Structure

    '' Used for more complex operations
    Public Structure QueryResponse(Of T)
        Public Successful As Boolean
        Public Information As String
        Public Data As T

        ''' <summary>
        ''' Returns the equivalent ValidationResponse from this structure
        ''' </summary>
        ''' <returns></returns>
        Public Function GetValidationData() As ValidationResponse
            Return New ValidationResponse With {.Successful = Successful, .Information = Information}
        End Function

        ''' <summary>
        ''' Returns the data with the HasInformation flag set whether this query was succesfull
        ''' </summary>
        ''' <returns></returns>
        Public Function GetStructData() As StructHolder(Of T)
            Return New StructHolder(Of T) With {.HasInformation = Successful, .Data = Data}
        End Function
    End Structure

    '' Used for holding a structure and possibly empty state
    '' Better than Nullable because doesn't use boxing
    '' (if it were it would be no different than a class except the memory would not be shared)
    Public Structure StructHolder(Of T)
        '' Set to true if Information has a meaning.
        '' Usefull in case it is an empty structure
        Public HasInformation As Boolean
        Public Data As T
    End Structure
End Module
