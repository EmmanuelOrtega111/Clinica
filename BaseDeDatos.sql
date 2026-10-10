CREATE DATABASE SistemaUnidadSalud;
GO

USE SistemaUnidadSalud;
GO

--Tabla de Pacientes
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
--Tabla de Consultas
CREATE TABLE Consultas (
    IdConsulta INT IDENTITY(1,1) PRIMARY KEY,
    NumeroExpediente VARCHAR(50) FOREIGN KEY REFERENCES Pacientes(NumeroExpediente),
    FechaConsulta DATETIME DEFAULT GETDATE(),
    ConsultaPor VARCHAR(MAX) NOT NULL,
    PresenteEnfermedad VARCHAR(MAX) NOT NULL,
    
    --Signos Vitales y Medidas Antropometricas
    TemperaturaCelsius DECIMAL(5,2),
    FrecuenciaRespiratoria INT,
    FrecuenciaCardiaca INT,
    Pulso INT,
    PresionArterial VARCHAR(20),
    SaturacionOxigeno DECIMAL(5,2),
    PesoKg DECIMAL(5,2),
    TallaCm DECIMAL(5,2),
    CircunferenciaAbdominalCm DECIMAL(5,2),
    
    --Diagnostico y Tratamiento
    TipoDiagnostico VARCHAR(20), -- Principal o Secundario
    DiagnosticoTexto VARCHAR(MAX),
    TipoConsulta VARCHAR(30), -- Primera vez o Subsecuente
    Recomendaciones VARCHAR(MAX)
);
GO

--Tabla de Examen Fisico
CREATE TABLE ExamenesFisicos (
    IdExamen INT IDENTITY(1,1) PRIMARY KEY,
    IdConsulta INT FOREIGN KEY REFERENCES Consultas(IdConsulta) ON DELETE CASCADE,
    Region VARCHAR(50) NOT NULL,
    EsNormal BIT NOT NULL,
    Detalle VARCHAR(MAX)
);
GO

--Tabla de Recetas
CREATE TABLE Recetas (
    IdReceta INT IDENTITY(1,1) PRIMARY KEY,
    IdConsulta INT FOREIGN KEY REFERENCES Consultas(IdConsulta) ON DELETE CASCADE,
    NombreMedicamento VARCHAR(150) NOT NULL,
    CantidadYDosis VARCHAR(150) NOT NULL
);
GO

--Tabla de Antecedentes Personales Patologicos
CREATE TABLE AntecedentesPersonales (
    IdAntecedente INT IDENTITY(1,1) PRIMARY KEY,
    NumeroExpediente VARCHAR(50) FOREIGN KEY REFERENCES Pacientes(NumeroExpediente),
    Enfermedad VARCHAR(150) NOT NULL,
    AnioInicio INT,
    TipoMedicacion VARCHAR(150)
);
GO

--Tabla de Datos Especificos para Paciente Embarazada
CREATE TABLE DetalleEmbarazada (
    IdConsulta INT FOREIGN KEY REFERENCES Consultas(IdConsulta) PRIMARY KEY,
    FormulaObstetrica VARCHAR(50),
    PesoAnteriorKg DECIMAL(5,2),
    EdadGestacionalTexto VARCHAR(50),
    FUR DATE,
    FPP DATE,
    --Datos de Perfil Prenatal resumidos
    GrupoSanguineo VARCHAR(10),
    VIH VARCHAR(MAX),
    Sifilis VARCHAR(MAX),
    UltrasonografiaObs VARCHAR(MAX)
);
GO

--Tabla de Datos Especificos para Adulto Mayor
CREATE TABLE DetalleAdultoMayor (
    IdConsulta INT FOREIGN KEY REFERENCES Consultas(IdConsulta) PRIMARY KEY,
    PuntajeBarthel INT,
    CategoriaDependencia VARCHAR(50)
);
GO