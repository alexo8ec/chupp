CREATE TABLE [dbo].[personas](
	[id_persona] [int] IDENTITY(1,1) NOT NULL,
	[nombre_persona] [varchar](50) NULL,
	[apellido_persona] [varchar](50) NULL,
	[cedula_persona] [varchar](50) NULL,
	[direccion_persona] [varchar](100) NULL,
	[email_persona] [varchar](50) NULL,
	[telefono_persona] [varchar](50) NULL,
	[celular_persona] [varchar](50) NULL,
	[estado_persona] [int] NULL,
	[fecha_nacimiento_persona] [date] NULL,
 CONSTRAINT [PK_personas] PRIMARY KEY CLUSTERED 
(
	[id_persona] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[porcentajes]    Script Date: 6/4/2022 10:49:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[porcentajes](
	[id_porcentaje] [int] IDENTITY(1,1) NOT NULL,
	[porcentaje] [varchar](50) NULL,
 CONSTRAINT [PK_porcentajes] PRIMARY KEY CLUSTERED 
(
	[id_porcentaje] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rangos]    Script Date: 6/4/2022 10:49:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rangos](
	[id_rango] [int] IDENTITY(1,1) NOT NULL,
	[rango] [varchar](50) NULL,
	[porcentaje] [decimal](18, 2) NULL,
 CONSTRAINT [PK_rengos] PRIMARY KEY CLUSTERED 
(
	[id_rango] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[seguros]    Script Date: 6/4/2022 10:49:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[seguros](
	[id_seguro] [int] IDENTITY(1,1) NOT NULL,
	[nombre_seguro] [varchar](50) NULL,
	[codigo_seguro] [varchar](50) NULL,
	[fecha_creacion_seguro] [datetime] NULL,
	[fecha_modificacion_seguro] [datetime] NULL,
	[rango_edad_seguro] [varchar](50) NULL,
	[porcentaje_seguro] [varchar](50) NULL,
	[id_tipo_seguro] [int] NULL,
	[valor_seguro] [decimal](18, 2) NULL,
	[estado_seguro] [int] NULL,
 CONSTRAINT [PK_seguros] PRIMARY KEY CLUSTERED 
(
	[id_seguro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tipo_seguros]    Script Date: 6/4/2022 10:49:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tipo_seguros](
	[id_tipo_seguro] [int] IDENTITY(1,1) NOT NULL,
	[tipo_seguro] [varchar](50) NULL,
	[estado_seguro] [int] NULL,
 CONSTRAINT [PK_tipo_seguros] PRIMARY KEY CLUSTERED 
(
	[id_tipo_seguro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ventas]    Script Date: 6/4/2022 10:49:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ventas](
	[id_venta] [int] IDENTITY(1,1) NOT NULL,
	[id_seguro_venta] [int] NULL,
	[id_persona_venta] [int] NULL,
	[fecha_creacion_venta] [datetime] NULL,
	[id_rango_venta] [int] NULL,
	[valor_venta] [decimal](18, 2) NULL,
	[id_porcentaje_venta] [int] NULL,
	[prima] [decimal](18, 2) NULL,
 CONSTRAINT [PK_ventas] PRIMARY KEY CLUSTERED 
(
	[id_venta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[personas] ON 

INSERT [dbo].[personas] ([id_persona], [nombre_persona], [apellido_persona], [cedula_persona], [direccion_persona], [email_persona], [telefono_persona], [celular_persona], [estado_persona], [fecha_nacimiento_persona]) VALUES (1, N'Alex Omar', N'Mite Salazar', N'0921605895', N'La 21 Entre Oriente y sedalana', N'alexo8ec@hotmail.com', N'042229388', N'0980631943', 1, CAST(N'1983-09-10' AS Date))
INSERT [dbo].[personas] ([id_persona], [nombre_persona], [apellido_persona], [cedula_persona], [direccion_persona], [email_persona], [telefono_persona], [celular_persona], [estado_persona], [fecha_nacimiento_persona]) VALUES (2, N'Anabel Victoria', N'Solorzano Gonzabay', N'0918064304', N'Los Esteros', N'anasolor27@hotmail.com', N'042229386', N'0983381246', 1, CAST(N'1981-03-15' AS Date))
INSERT [dbo].[personas] ([id_persona], [nombre_persona], [apellido_persona], [cedula_persona], [direccion_persona], [email_persona], [telefono_persona], [celular_persona], [estado_persona], [fecha_nacimiento_persona]) VALUES (3, N'Alan Omar', N'Mite Solorzano', N'0943897025', N'Coop 7 lagos', N'alan8ec@hotmail.com', N'042545789', N'0945698785', 1, CAST(N'2003-09-04' AS Date))
SET IDENTITY_INSERT [dbo].[personas] OFF
GO
SET IDENTITY_INSERT [dbo].[porcentajes] ON 

INSERT [dbo].[porcentajes] ([id_porcentaje], [porcentaje]) VALUES (1, N'20-80')
INSERT [dbo].[porcentajes] ([id_porcentaje], [porcentaje]) VALUES (2, N'10-90')
INSERT [dbo].[porcentajes] ([id_porcentaje], [porcentaje]) VALUES (3, N'100-0')
SET IDENTITY_INSERT [dbo].[porcentajes] OFF
GO
SET IDENTITY_INSERT [dbo].[rangos] ON 

INSERT [dbo].[rangos] ([id_rango], [rango], [porcentaje]) VALUES (1, N'15-20', CAST(5.00 AS Decimal(18, 2)))
INSERT [dbo].[rangos] ([id_rango], [rango], [porcentaje]) VALUES (2, N'21-30', CAST(10.00 AS Decimal(18, 2)))
INSERT [dbo].[rangos] ([id_rango], [rango], [porcentaje]) VALUES (3, N'31-40', CAST(15.00 AS Decimal(18, 2)))
INSERT [dbo].[rangos] ([id_rango], [rango], [porcentaje]) VALUES (4, N'41-80', CAST(20.00 AS Decimal(18, 2)))
INSERT [dbo].[rangos] ([id_rango], [rango], [porcentaje]) VALUES (5, N'80-0', CAST(25.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[rangos] OFF
GO
SET IDENTITY_INSERT [dbo].[seguros] ON 

INSERT [dbo].[seguros] ([id_seguro], [nombre_seguro], [codigo_seguro], [fecha_creacion_seguro], [fecha_modificacion_seguro], [rango_edad_seguro], [porcentaje_seguro], [id_tipo_seguro], [valor_seguro], [estado_seguro]) VALUES (6, N'Seguro Vehícular Monte', N'AU0000004', CAST(N'2022-06-04T13:24:20.000' AS DateTime), CAST(N'2022-06-04T19:52:48.323' AS DateTime), NULL, NULL, 3, CAST(30.00 AS Decimal(18, 2)), 1)
INSERT [dbo].[seguros] ([id_seguro], [nombre_seguro], [codigo_seguro], [fecha_creacion_seguro], [fecha_modificacion_seguro], [rango_edad_seguro], [porcentaje_seguro], [id_tipo_seguro], [valor_seguro], [estado_seguro]) VALUES (7, N'Seguro Ambulatorio', N'ME0000008', CAST(N'2022-06-04T13:35:19.000' AS DateTime), CAST(N'2022-06-04T13:37:17.380' AS DateTime), NULL, NULL, 1, CAST(25.00 AS Decimal(18, 2)), 1)
INSERT [dbo].[seguros] ([id_seguro], [nombre_seguro], [codigo_seguro], [fecha_creacion_seguro], [fecha_modificacion_seguro], [rango_edad_seguro], [porcentaje_seguro], [id_tipo_seguro], [valor_seguro], [estado_seguro]) VALUES (8, N'Seguro mi Auto', N'AU00000011', CAST(N'2022-06-04T13:38:13.810' AS DateTime), CAST(N'2022-06-04T13:38:13.810' AS DateTime), NULL, NULL, 3, CAST(20.00 AS Decimal(18, 2)), 1)
INSERT [dbo].[seguros] ([id_seguro], [nombre_seguro], [codigo_seguro], [fecha_creacion_seguro], [fecha_modificacion_seguro], [rango_edad_seguro], [porcentaje_seguro], [id_tipo_seguro], [valor_seguro], [estado_seguro]) VALUES (9, N'Seguro médico mis niños', N'ME000010', CAST(N'2022-06-04T13:41:41.950' AS DateTime), CAST(N'2022-06-04T13:41:41.960' AS DateTime), NULL, NULL, 1, CAST(30.00 AS Decimal(18, 2)), 1)
SET IDENTITY_INSERT [dbo].[seguros] OFF
GO
SET IDENTITY_INSERT [dbo].[tipo_seguros] ON 

INSERT [dbo].[tipo_seguros] ([id_tipo_seguro], [tipo_seguro], [estado_seguro]) VALUES (1, N'Médico', 1)
INSERT [dbo].[tipo_seguros] ([id_tipo_seguro], [tipo_seguro], [estado_seguro]) VALUES (2, N'Vida', 1)
INSERT [dbo].[tipo_seguros] ([id_tipo_seguro], [tipo_seguro], [estado_seguro]) VALUES (3, N'Automotriz', 1)
SET IDENTITY_INSERT [dbo].[tipo_seguros] OFF
GO
SET IDENTITY_INSERT [dbo].[ventas] ON 

INSERT [dbo].[ventas] ([id_venta], [id_seguro_venta], [id_persona_venta], [fecha_creacion_venta], [id_rango_venta], [valor_venta], [id_porcentaje_venta], [prima]) VALUES (2, 6, 3, CAST(N'2022-06-04T22:27:56.303' AS DateTime), 1, CAST(31.50 AS Decimal(18, 2)), 1, CAST(36.00 AS Decimal(18, 2)))
INSERT [dbo].[ventas] ([id_venta], [id_seguro_venta], [id_persona_venta], [fecha_creacion_venta], [id_rango_venta], [valor_venta], [id_porcentaje_venta], [prima]) VALUES (3, 9, 3, CAST(N'2022-06-04T22:29:21.323' AS DateTime), 1, CAST(31.50 AS Decimal(18, 2)), 2, CAST(37.50 AS Decimal(18, 2)))
INSERT [dbo].[ventas] ([id_venta], [id_seguro_venta], [id_persona_venta], [fecha_creacion_venta], [id_rango_venta], [valor_venta], [id_porcentaje_venta], [prima]) VALUES (4, 8, 3, CAST(N'2022-06-04T22:33:18.217' AS DateTime), 1, CAST(21.00 AS Decimal(18, 2)), 3, CAST(25.20 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[ventas] OFF
GO
