CREATE DATABASE SistemaUnidadSalud;
GO

USE SistemaUnidadSalud;
GO

--Tablas principales

--Tabla de pacientes
CREATE TABLE Pacientes (
    NumeroExpediente VARCHAR(50) PRIMARY KEY,
    Nombre VARCHAR(150) NOT NULL,
    Edad INT NOT NULL,
    Sexo VARCHAR(20) NOT NULL,
    Direccion VARCHAR(250) NOT NULL,
    Responsable VARCHAR(150) NOT NULL,
    TipoPaciente VARCHAR(50) NOT NULL
);
GO

--Tabla de consultas
CREATE TABLE Consultas (
    IdConsulta INT IDENTITY(1,1) PRIMARY KEY,
    NumeroExpediente VARCHAR(50) FOREIGN KEY REFERENCES Pacientes(NumeroExpediente),
    FechaConsulta DATETIME DEFAULT GETDATE(),
    ConsultaPor VARCHAR(MAX) NOT NULL,
    PresenteEnfermedad VARCHAR(MAX) NOT NULL,
 
    TemperaturaCelsius DECIMAL(5,2),
    FrecuenciaRespiratoria INT,
    FrecuenciaCardiaca INT,
    Pulso INT,
    PresionArterial VARCHAR(20),
    SaturacionOxigeno DECIMAL(5,2),
    PesoKg DECIMAL(5,2),
    TallaCm DECIMAL(5,2),
    CircunferenciaAbdominalCm DECIMAL(5,2),
    
    TipoDiagnostico VARCHAR(20),
    DiagnosticoTexto VARCHAR(MAX),
    TipoConsulta VARCHAR(30),
    Recomendaciones VARCHAR(MAX)
);
GO

--Tabla de examen fisico
CREATE TABLE ExamenesFisicos (
    IdExamen INT IDENTITY(1,1) PRIMARY KEY,
    IdConsulta INT FOREIGN KEY REFERENCES Consultas(IdConsulta) ON DELETE CASCADE,
    Region VARCHAR(50) NOT NULL,
    EsNormal BIT NOT NULL,
    Detalle VARCHAR(MAX)
);
GO

--Tabla de recetas
CREATE TABLE Recetas (
    IdReceta INT IDENTITY(1,1) PRIMARY KEY,
    IdConsulta INT FOREIGN KEY REFERENCES Consultas(IdConsulta) ON DELETE CASCADE,
    IndicacionesMedicamento VARCHAR(MAX) NOT NULL
);
GO

--Tablas de detalles y subclases

--Detalle para adulto mayor
CREATE TABLE DetalleAdultoMayor (
    IdConsulta INT NOT NULL PRIMARY KEY,
    PuntajeBarthel INT NOT NULL DEFAULT 0,
    CategoriaDependencia VARCHAR(50) NULL,
    CONSTRAINT FK_DetalleAdultoMayor_Consultas FOREIGN KEY (IdConsulta) 
        REFERENCES dbo.Consultas(IdConsulta) ON DELETE CASCADE
);
GO

--Detalle para paciente infantil
CREATE TABLE DetalleInfantil (
    IdConsulta INT NOT NULL PRIMARY KEY,
    VitaminaASuministrada BIT NOT NULL DEFAULT 0,
    MultivitaminasSuministradas BIT NOT NULL DEFAULT 0,
    HierroSuministrado BIT NOT NULL DEFAULT 0,
    MebendazolSuministrado BIT NOT NULL DEFAULT 0,
    AlbendazolSuministrado BIT NOT NULL DEFAULT 0,
    EsquemaVacunacionCompleto BIT NOT NULL DEFAULT 0,
    SufreMaltratoViolencia BIT NOT NULL DEFAULT 0,
    CumpleDerechosNino BIT NOT NULL DEFAULT 0,
    PresentaRiesgoSocial BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_DetalleInfantil_Consultas FOREIGN KEY (IdConsulta) 
        REFERENCES dbo.Consultas(IdConsulta) ON DELETE CASCADE
);
GO
--Detalle para paciente adolescente
CREATE TABLE DetalleAdolescente (
    IdDetalleAdolescente INT IDENTITY(1,1) PRIMARY KEY,
    IdConsulta INT NOT NULL,

    --Estilos de vida/consumo
    Etilista BIT NOT NULL,
    Tabaquista BIT NOT NULL,
    Drogadiccion BIT NOT NULL,

    --Evaluaciones adicionales
    EscalaTanner VARCHAR(50) NULL,
    PanelSnellen VARCHAR(50) NULL,

    -- Antecedentes Familiares Patológicos
    AntFam_Hipertension BIT NOT NULL DEFAULT 0,
    AntFam_TrastornosMentales BIT NOT NULL DEFAULT 0,
    AntFam_RenalCronica BIT NOT NULL DEFAULT 0,
    AntFam_Infecciones BIT NOT NULL DEFAULT 0,
    AntFam_Obesidad BIT NOT NULL DEFAULT 0,
    AntFam_Cardiovasculares BIT NOT NULL DEFAULT 0,
    AntFam_Diabetes BIT NOT NULL DEFAULT 0,
    AntFam_Asma BIT NOT NULL DEFAULT 0,
    AntFam_Cancer BIT NOT NULL DEFAULT 0,

    --Antecedentes personales patologicos 
    AntPers_Hipertension_Anio INT NULL, AntPers_Hipertension_Med VARCHAR(150) NULL,
    AntPers_Cardio_Anio INT NULL, AntPers_Cardio_Med VARCHAR(150) NULL,
    AntPers_Epilepsia_Anio INT NULL, AntPers_Epilepsia_Med VARCHAR(150) NULL,
    AntPers_EPOC_Anio INT NULL, AntPers_EPOC_Med VARCHAR(150) NULL,
    AntPers_Infecto_Anio INT NULL, AntPers_Infecto_Med VARCHAR(150) NULL,
    AntPers_Cancer_Anio INT NULL, AntPers_Cancer_Med VARCHAR(150) NULL,
    AntPers_Asma_Anio INT NULL, AntPers_Asma_Med VARCHAR(150) NULL,
    AntPers_Mentales_Anio INT NULL, AntPers_Mentales_Med VARCHAR(150) NULL,
    AntPers_Artritis_Anio INT NULL, AntPers_Artritis_Med VARCHAR(150) NULL,
    AntPers_Obesidad_Anio INT NULL, AntPers_Obesidad_Med VARCHAR(150) NULL,

    --Solicitud de laboratorios especificos 
    Lab_Hemograma BIT NOT NULL DEFAULT 0,
    Lab_Colesterol BIT NOT NULL DEFAULT 0,
    Lab_Trigliceridos BIT NOT NULL DEFAULT 0,
    Lab_Heces BIT NOT NULL DEFAULT 0,
    Lab_Orina BIT NOT NULL DEFAULT 0,

    CONSTRAINT FK_DetalleAdolescente_Consultas FOREIGN KEY (IdConsulta) 
        REFERENCES dbo.Consultas(IdConsulta) ON DELETE CASCADE
);
GO

-- Detalle para Paciente Embarazada
CREATE TABLE DetalleEmbarazada (
    IdConsulta INT NOT NULL PRIMARY KEY,
    
    --Gestación y datos obstétricos
    FormulaObstetrica VARCHAR(50) NULL,
    FinEmbarazoAntFecha VARCHAR(50) NULL,
    FinEmbarazoAntTerminacion VARCHAR(100) NULL,
    EmbarazoNoDeseado BIT NOT NULL DEFAULT 0,
    UsabaMetodoAnticonceptivo BIT NOT NULL DEFAULT 0,
    MetodoAnticonceptivoEmpleado VARCHAR(100) NULL,
    PesoAnteriorKg DECIMAL(5,2) NULL,
    EdadGestacionalTexto VARCHAR(50) NULL,
    FUR DATE NULL,
    FPP DATE NULL,

    --Habitos y riesgos por Trimestre
    T1_FumaActivamente BIT NOT NULL DEFAULT 0,
    T1_FumaPasivamente BIT NOT NULL DEFAULT 0,
    T1_Drogas BIT NOT NULL DEFAULT 0,
    T1_Alcohol BIT NOT NULL DEFAULT 0,
    T1_Violencia BIT NOT NULL DEFAULT 0,

    T2_FumaActivamente BIT NOT NULL DEFAULT 0,
    T2_FumaPasivamente BIT NOT NULL DEFAULT 0,
    T2_Drogas BIT NOT NULL DEFAULT 0,
    T2_Alcohol BIT NOT NULL DEFAULT 0,
    T2_Violencia BIT NOT NULL DEFAULT 0,

    T3_FumaActivamente BIT NOT NULL DEFAULT 0,
    T3_FumaPasivamente BIT NOT NULL DEFAULT 0,
    T3_Drogas BIT NOT NULL DEFAULT 0,
    T3_Alcohol BIT NOT NULL DEFAULT 0,
    T3_Violencia BIT NOT NULL DEFAULT 0,

    --Cartilla de vacunacion
    VacTetanosAplicada BIT NOT NULL DEFAULT 0,
    FechaTetanos VARCHAR(50) NULL,
    VacTdpaAplicada BIT NOT NULL DEFAULT 0,
    FechaTdpa VARCHAR(50) NULL,
    VacInfluenzaAplicada BIT NOT NULL DEFAULT 0,
    FechaInfluenza VARCHAR(50) NULL,
    VacRubeolaAplicada BIT NOT NULL DEFAULT 0,
    FechaRubeola VARCHAR(50) NULL,
    VacHepBAplicada BIT NOT NULL DEFAULT 0,
    FechaHepB VARCHAR(50) NULL,
    VacHepAAplicada BIT NOT NULL DEFAULT 0,
    FechaHepA VARCHAR(50) NULL,
    VacCovidAplicada BIT NOT NULL DEFAULT 0,
    FechaCovid VARCHAR(50) NULL,

    --Perfil Prenatal
    CitologiaFechaToma VARCHAR(50) NULL,
    CitologiaFechaResultado VARCHAR(50) NULL,
    CitologiaResultado VARCHAR(MAX) NULL,
    GrupoSanguineo VARCHAR(20) NULL,
    HemoglobinaMenor20Sem VARCHAR(50) NULL,
    HemoglobinaMayor20Sem VARCHAR(50) NULL,
    Chagas VARCHAR(50) NULL,
    BacteriuriaMenor20Sem VARCHAR(50) NULL,
    BacteriuriaMayor20Sem VARCHAR(50) NULL,
    GlucemiaAyunoMenor30Sem VARCHAR(50) NULL,
    EstreptococosB_30_37Sem VARCHAR(50) NULL,
    PruebaVIH VARCHAR(MAX) NULL,
    PruebaSifilis VARCHAR(MAX) NULL,
    UltrasonografiaObs VARCHAR(MAX) NULL,

    CONSTRAINT FK_DetalleEmbarazada_Consultas FOREIGN KEY (IdConsulta) 
        REFERENCES dbo.Consultas(IdConsulta) ON DELETE CASCADE
);
GO

--Detalle para Mujer en Edad Fertil
CREATE TABLE DetalleMujerEdadFertil (
    IdConsulta INT NOT NULL PRIMARY KEY,
    MetodoAnticonceptivo VARCHAR(100) NULL,
    FechaUltimaMenstruacion VARCHAR(50) NULL,
    FechaUltimoParto VARCHAR(50) NULL,
    FormulaObstetrica VARCHAR(50) NULL,
    CONSTRAINT FK_DetalleMujerEdadFertil_Consultas FOREIGN KEY (IdConsulta) 
        REFERENCES dbo.Consultas(IdConsulta) ON DELETE CASCADE
);
GO
--Detalle Crónico
CREATE TABLE DetalleCronico (
    IdConsulta INT FOREIGN KEY REFERENCES Consultas(IdConsulta) PRIMARY KEY,
    ComplicacionesRecurrentes VARCHAR(MAX),
    ApegadoTratamiento BIT
);
GO
--Tabla de ususarios y datos iniciales
CREATE TABLE Usuarios (
    idUsuario INT PRIMARY KEY IDENTITY(1,1),
    NombreUs VARCHAR(50) NOT NULL UNIQUE,
    contra VARCHAR(50) NOT NULL,
    Rol VARCHAR(20) NOT NULL,
    Activo BIT NOT NULL DEFAULT 1
);
GO

--Insertar Usuarios de Prueba
INSERT INTO Usuarios (NombreUs, contra, Rol, Activo) 
VALUES 
    ('Carlos', 'Admin123', 'Administrador', 1),
    ('Sofi', 'Doc123', 'Médico', 1);
GO
--Vistas
CREATE OR ALTER VIEW vw_DetalleInfantilLegible AS
SELECT 
    IdConsulta,
    CASE WHEN VitaminaASuministrada = 1 THEN 'Sí' ELSE 'No' END AS VitaminaASuministrada,
    CASE WHEN MultivitaminasSuministradas = 1 THEN 'Sí' ELSE 'No' END AS MultivitaminasSuministradas,
    CASE WHEN HierroSuministrado = 1 THEN 'Sí' ELSE 'No' END AS HierroSuministrado,
    CASE WHEN MebendazolSuministrado = 1 THEN 'Sí' ELSE 'No' END AS MebendazolSuministrado,
    CASE WHEN AlbendazolSuministrado = 1 THEN 'Sí' ELSE 'No' END AS AlbendazolSuministrado,
    CASE WHEN EsquemaVacunacionCompleto = 1 THEN 'Sí' ELSE 'No' END AS EsquemaVacunacionCompleto,
    CASE WHEN SufreMaltratoViolencia = 1 THEN 'Sí' ELSE 'No' END AS SufreMaltratoViolencia,
    CASE WHEN CumpleDerechosNino = 1 THEN 'Sí' ELSE 'No' END AS CumpleDerechosNino,
    CASE WHEN PresentaRiesgoSocial = 1 THEN 'Sí' ELSE 'No' END AS PresentaRiesgoSocial
FROM dbo.DetalleInfantil;
GO

CREATE OR ALTER VIEW vw_DetalleAdolescenteLegible AS
SELECT 
    IdConsulta,
    CASE WHEN Etilista = 1 THEN 'Sí' ELSE 'No' END AS Etilista,
    CASE WHEN Tabaquista = 1 THEN 'Sí' ELSE 'No' END AS Tabaquista,
    CASE WHEN Drogadiccion = 1 THEN 'Sí' ELSE 'No' END AS Drogadiccion,
    EscalaTanner,
    PanelSnellen,
    CASE WHEN AntFam_Hipertension = 1 THEN 'Sí' ELSE 'No' END AS AntFam_Hipertension,
    CASE WHEN AntFam_TrastornosMentales = 1 THEN 'Sí' ELSE 'No' END AS AntFam_TrastornosMentales,
    CASE WHEN AntFam_RenalCronica = 1 THEN 'Sí' ELSE 'No' END AS AntFam_RenalCronica,
    CASE WHEN AntFam_Infecciones = 1 THEN 'Sí' ELSE 'No' END AS AntFam_Infecciones,
    CASE WHEN AntFam_Obesidad = 1 THEN 'Sí' ELSE 'No' END AS AntFam_Obesidad,
    CASE WHEN AntFam_Cardiovasculares = 1 THEN 'Sí' ELSE 'No' END AS AntFam_Cardiovasculares,
    CASE WHEN AntFam_Diabetes = 1 THEN 'Sí' ELSE 'No' END AS AntFam_Diabetes,
    CASE WHEN AntFam_Asma = 1 THEN 'Sí' ELSE 'No' END AS AntFam_Asma,
    CASE WHEN AntFam_Cancer = 1 THEN 'Sí' ELSE 'No' END AS AntFam_Cancer,
    AntPers_Hipertension_Anio, AntPers_Hipertension_Med,
    AntPers_Cardio_Anio, AntPers_Cardio_Med,
    AntPers_Epilepsia_Anio, AntPers_Epilepsia_Med,
    AntPers_EPOC_Anio, AntPers_EPOC_Med,
    AntPers_Infecto_Anio, AntPers_Infecto_Med,
    AntPers_Cancer_Anio, AntPers_Cancer_Med,
    AntPers_Asma_Anio, AntPers_Asma_Med,
    AntPers_Mentales_Anio, AntPers_Mentales_Med,
    AntPers_Artritis_Anio, AntPers_Artritis_Med,
    AntPers_Obesidad_Anio, AntPers_Obesidad_Med,
    CASE WHEN Lab_Hemograma = 1 THEN 'Sí' ELSE 'No' END AS Lab_Hemograma,
    CASE WHEN Lab_Colesterol = 1 THEN 'Sí' ELSE 'No' END AS Lab_Colesterol,
    CASE WHEN Lab_Trigliceridos = 1 THEN 'Sí' ELSE 'No' END AS Lab_Trigliceridos,
    CASE WHEN Lab_Heces = 1 THEN 'Sí' ELSE 'No' END AS Lab_Heces,
    CASE WHEN Lab_Orina = 1 THEN 'Sí' ELSE 'No' END AS Lab_Orina
FROM dbo.DetalleAdolescente;
GO

CREATE OR ALTER VIEW vw_DetalleEmbarazadaLegible AS
SELECT 
    IdConsulta,
    FormulaObstetrica,
    FinEmbarazoAntFecha,
    FinEmbarazoAntTerminacion,
    CASE WHEN EmbarazoNoDeseado = 1 THEN 'Sí' ELSE 'No' END AS EmbarazoNoDeseado,
    CASE WHEN UsabaMetodoAnticonceptivo = 1 THEN 'Sí' ELSE 'No' END AS UsabaMetodoAnticonceptivo,
    MetodoAnticonceptivoEmpleado,
    PesoAnteriorKg,
    EdadGestacionalTexto,
    FUR,
    FPP,
    CASE WHEN T1_FumaActivamente = 1 THEN 'Sí' ELSE 'No' END AS T1_FumaActivamente,
    CASE WHEN T1_FumaPasivamente = 1 THEN 'Sí' ELSE 'No' END AS T1_FumaPasivamente,
    CASE WHEN T1_Drogas = 1 THEN 'Sí' ELSE 'No' END AS T1_Drogas,
    CASE WHEN T1_Alcohol = 1 THEN 'Sí' ELSE 'No' END AS T1_Alcohol,
    CASE WHEN T1_Violencia = 1 THEN 'Sí' ELSE 'No' END AS T1_Violencia,
    CASE WHEN T2_FumaActivamente = 1 THEN 'Sí' ELSE 'No' END AS T2_FumaActivamente,
    CASE WHEN T2_FumaPasivamente = 1 THEN 'Sí' ELSE 'No' END AS T2_FumaPasivamente,
    CASE WHEN T2_Drogas = 1 THEN 'Sí' ELSE 'No' END AS T2_Drogas,
    CASE WHEN T2_Alcohol = 1 THEN 'Sí' ELSE 'No' END AS T2_Alcohol,
    CASE WHEN T2_Violencia = 1 THEN 'Sí' ELSE 'No' END AS T2_Violencia,
    CASE WHEN T3_FumaActivamente = 1 THEN 'Sí' ELSE 'No' END AS T3_FumaActivamente,
    CASE WHEN T3_FumaPasivamente = 1 THEN 'Sí' ELSE 'No' END AS T3_FumaPasivamente,
    CASE WHEN T3_Drogas = 1 THEN 'Sí' ELSE 'No' END AS T3_Drogas,
    CASE WHEN T3_Alcohol = 1 THEN 'Sí' ELSE 'No' END AS T3_Alcohol,
    CASE WHEN T3_Violencia = 1 THEN 'Sí' ELSE 'No' END AS T3_Violencia,
    CASE WHEN VacTetanosAplicada = 1 THEN 'Sí' ELSE 'No' END AS VacTetanosAplicada,
    FechaTetanos,
    CASE WHEN VacTdpaAplicada = 1 THEN 'Sí' ELSE 'No' END AS VacTdpaAplicada,
    FechaTdpa,
    CASE WHEN VacInfluenzaAplicada = 1 THEN 'Sí' ELSE 'No' END AS VacInfluenzaAplicada,
    FechaInfluenza,
    CASE WHEN VacRubeolaAplicada = 1 THEN 'Sí' ELSE 'No' END AS VacRubeolaAplicada,
    FechaRubeola,
    CASE WHEN VacHepBAplicada = 1 THEN 'Sí' ELSE 'No' END AS VacHepBAplicada,
    FechaHepB,
    CASE WHEN VacHepAAplicada = 1 THEN 'Sí' ELSE 'No' END AS VacHepAAplicada,
    FechaHepA,
    CASE WHEN VacCovidAplicada = 1 THEN 'Sí' ELSE 'No' END AS VacCovidAplicada,
    FechaCovid,
    CitologiaFechaToma,
    CitologiaFechaResultado,
    CitologiaResultado,
    GrupoSanguineo,
    HemoglobinaMenor20Sem,
    HemoglobinaMayor20Sem,
    Chagas,
    BacteriuriaMenor20Sem,
    BacteriuriaMayor20Sem,
    GlucemiaAyunoMenor30Sem,
    EstreptococosB_30_37Sem,
    PruebaVIH,
    PruebaSifilis,
    UltrasonografiaObs
FROM dbo.DetalleEmbarazada;
GO