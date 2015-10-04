SET IDENTITY_INSERT [dbo].[Diary] ON
IF NOT EXISTS(SELECT * FROM  Diary)
BEGIN
INSERT INTO [dbo].[Diary] ([Id], [PostDateTime], [Content]) VALUES (1016, N'2015-09-10 14:50:52', N'{\rtf1\ansi\ansicpg1252\uc1\htmautsp\deff2{\fonttbl{\f0\fcharset0 Times New Roman;}{\f2\fcharset0 Segoe UI;}}{\colortbl\red0\green0\blue0;\red255\green255\blue255;}\loch\hich\dbch\pard\plain\ltrpar\itap0{\lang1033\fs18\f2\cf0 \cf0\ql{\f2 {\ltrch What are you thinking about?}\li0\ri0\sa0\sb0\fi0\ql\par}
}
}')
INSERT INTO [dbo].[Diary] ([Id], [PostDateTime], [Content]) VALUES (4041, N'2015-09-11 07:56:45', N'{\rtf1\ansi\ansicpg1252\uc1\htmautsp\deff2{\fonttbl{\f0\fcharset0 Times New Roman;}{\f2\fcharset0 Segoe UI;}}{\colortbl\red0\green0\blue0;\red255\green255\blue255;}\loch\hich\dbch\pard\plain\ltrpar\itap0{\lang1033\fs18\f2\cf0 \cf0\ql{\f2 {\ltrch Life is like riding a bicycle.}\li0\ri0\sa0\sb0\fi0\ql\par}
{\f2 {\ltrch To keep your balance, you must keep moving on}\li0\ri0\sa0\sb0\fi0\ql\par}
}
}')
INSERT INTO [dbo].[Diary] ([Id], [PostDateTime], [Content]) VALUES (4050, N'2014-09-10 23:02:27', N'Hello
')
INSERT INTO [dbo].[Diary] ([Id], [PostDateTime], [Content]) VALUES (4054, N'2015-09-11 07:47:04', N'{\rtf1\ansi\ansicpg1252\uc1\htmautsp\deff2{\fonttbl{\f0\fcharset0 Times New Roman;}{\f2\fcharset0 Segoe UI;}{\f3\fcharset0 Baskerville Old Face;}}{\colortbl\red0\green0\blue0;\red255\green255\blue255;}\loch\hich\dbch\pard\plain\ltrpar\itap0{\lang1033\fs40\f3\cf0 \cf0\ql{\f3\b {\ltrch To be or not to be?}\li0\ri0\sa0\sb0\fi0\ql\par}
}
}')

INSERT INTO [dbo].[Diary] ([Id], [PostDateTime], [Content]) VALUES (4061, N'2015-09-11 00:57:13', N'{\rtf1\ansi\ansicpg1252\uc1\htmautsp\deff2{\fonttbl{\f0\fcharset0 Times New Roman;}{\f2\fcharset0 Segoe UI;}}{\colortbl\red0\green0\blue0;\red255\green255\blue255;}\loch\hich\dbch\pard\plain\ltrpar\itap0{\lang1033\fs18\f2\cf0 \cf0\ql{\f2 {\ltrch Hello world}\li0\ri0\sa0\sb0\fi0\ql\par}
{\f2 {\ltrch }\li0\ri0\sa0\sb0\fi0\ql\par}
}
}')
INSERT INTO [dbo].[Diary] ([Id], [PostDateTime], [Content]) VALUES (4063, N'2015-09-11 07:51:08', N'{\rtf1\ansi\ansicpg1252\uc1\htmautsp\deff2{\fonttbl{\f0\fcharset0 Times New Roman;}{\f2\fcharset0 Segoe UI;}{\f3\fcharset0 Consolas;}}{\colortbl\red0\green0\blue0;\red255\green255\blue255;\red0\green0\blue255;\red163\green21\blue21;}\loch\hich\dbch\pard\plain\ltrpar\itap0{\lang1033\fs18\f2\cf0 \cf0\ql{\f2 {\ltrch my first app}\li0\ri0\sa0\sb0\fi0\ql\par}
{\f2 \li0\ri0\sa0\sb0\fi0\ql\par}
{\fs19\f3 {\cf2\highlight1\ltrch #include}{\highlight1\ltrch  }{\cf3\highlight1\ltrch <iostream>}\li0\ri0\sa0\sb0\fi0\ql\par}
{\fs19\f3 {\cf2\highlight1\ltrch using}{\highlight1\ltrch  }{\cf2\highlight1\ltrch namespace}{\highlight1\ltrch  std;}\li0\ri0\sa0\sb0\fi0\ql\par}
{\fs19\f3 \li0\ri0\sa0\sb0\fi0\ql\par}
{\fs19\f3 \li0\ri0\sa0\sb0\fi0\ql\par}
{\fs19\f3 {\cf2\highlight1\ltrch int}{\highlight1\ltrch  main()}\li0\ri0\sa0\sb0\fi0\ql\par}
{\fs19\f3 {\highlight1\ltrch \{}\li0\ri0\sa0\sb0\fi0\ql\par}
{\fs19\f3 {\highlight1\ltrch \tab cout << }{\cf3\highlight1\ltrch "Hello world"}{\highlight1\ltrch  << endl;}\li0\ri0\sa0\sb0\fi0\ql\par}
{\fs19\f3 \li0\ri0\sa0\sb0\fi0\ql\par}
{\fs19\f3 \li0\ri0\sa0\sb0\fi0\ql\par}
{\fs19\f3 {\highlight1\ltrch     }{\cf2\highlight1\ltrch return}{\highlight1\ltrch  0;}\li0\ri0\sa0\sb0\fi0\ql\par}
{\fs19\f3 {\highlight1\ltrch \}}\li0\ri0\sa0\sb0\fi0\ql\par}
}
}')
SET IDENTITY_INSERT [dbo].[Diary] OFF
END;