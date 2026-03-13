using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AppPagarRecibo.Models;

namespace AppPagarRecibo.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "apppagar.db3");
            _database = new SQLiteAsyncConnection(dbPath);
        }

        public async Task InicializarAsync()
        {
            await _database.CreateTableAsync<Usuario>();
            await _database.CreateTableAsync<TipoUsuario>();
            await _database.CreateTableAsync<Carrera>();
            await _database.CreateTableAsync<Beca>();
            await _database.CreateTableAsync<UsuarioTipoAdeudo>();
            await _database.CreateTableAsync<TipoAdeudo>();
            await _database.CreateTableAsync<Transaccion>();
            await _database.CreateTableAsync<Banco>();
            await _database.CreateTableAsync<TipoOportunidad>();
            await _database.CreateTableAsync<Oportunidad>();
            await _database.CreateTableAsync<Asignatura>();
            await _database.CreateTableAsync<PeriodoSemestral>();
            await _database.CreateTableAsync<UsuarioPeriodoSemestral>();

            // Solo insertar seed data si la BD está vacía
            var count = await _database.Table<TipoUsuario>().CountAsync();
            if (count == 0)
                await InsertarDatosSemillaAsync();
        }

        private async Task InsertarDatosSemillaAsync()
        {
            var ahora = DateTime.Now;

            // Tipos de usuario
            await _database.InsertAsync(new TipoUsuario { Id = 1, Descripcion = "Nuevo Ingreso", FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });
            await _database.InsertAsync(new TipoUsuario { Id = 2, Descripcion = "Reingreso", FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });

            // Carreras
            await _database.InsertAsync(new Carrera { Id = 1, Clave = "IIF", Descripcion = "INGENIERÍA INFORMÁTICA", FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });
            await _database.InsertAsync(new Carrera { Id = 2, Clave = "IIN", Descripcion = "INGENIERÍA INDUSTRIAL", FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });
            await _database.InsertAsync(new Carrera { Id = 3, Clave = "ISC", Descripcion = "INGENIERÍA EN SISTEMAS", FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });

            // Becas
            await _database.InsertAsync(new Beca { Id = 1, Descripcion = "Beca Académica", Porcentaje = 30, FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });
            await _database.InsertAsync(new Beca { Id = 2, Descripcion = "Beca Deportiva", Porcentaje = 50, FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });
            await _database.InsertAsync(new Beca { Id = 3, Descripcion = "Beca Manutención", Porcentaje = 100, FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });

            // Banco receptor (Scotiabank como en el recibo real del TEC)
            await _database.InsertAsync(new Banco
            {
                Id = 1,
                Descripcion = "Scotiabank",
                CuentaDeposito = "18804618335",
                CLABE = "646218162649940000",
                BancoReceptor = "STP",
                InstruccionesSPEI = "1. Entra a tu banca en línea\n2. Selecciona: Transferencia a otros bancos\n3. Banco receptor: Sistemas de Transferencias y Pagos (STP)\n4. Captura los 18 dígitos de la CLABE\n5. En concepto: Tu nombre y matrícula\n6. Captura el monto a pagar\n7. Confirma tu pago",
                FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1
            });

            // Tipos de oportunidad
            await _database.InsertAsync(new TipoOportunidad { Id = 1, Clave = 0, Descripcion = "Traslado", GeneraCostoExtra = false, EsEspecial = false, FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });
            await _database.InsertAsync(new TipoOportunidad { Id = 2, Clave = 1, Descripcion = "Ordinario", GeneraCostoExtra = false, EsEspecial = false, FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });
            await _database.InsertAsync(new TipoOportunidad { Id = 3, Clave = 2, Descripcion = "Regularización", GeneraCostoExtra = false, EsEspecial = false, FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });
            await _database.InsertAsync(new TipoOportunidad { Id = 4, Clave = 3, Descripcion = "Ordinario Repite", GeneraCostoExtra = false, EsEspecial = false, FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });
            await _database.InsertAsync(new TipoOportunidad { Id = 5, Clave = 4, Descripcion = "Regularización Repite", GeneraCostoExtra = false, EsEspecial = false, FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });
            await _database.InsertAsync(new TipoOportunidad { Id = 6, Clave = 5, Descripcion = "Ordinario Especial", GeneraCostoExtra = true, EsEspecial = true, FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });
            await _database.InsertAsync(new TipoOportunidad { Id = 7, Clave = 6, Descripcion = "Regularización Especial", GeneraCostoExtra = true, EsEspecial = true, FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });
            await _database.InsertAsync(new TipoOportunidad { Id = 8, Clave = 7, Descripcion = "Global", GeneraCostoExtra = true, EsEspecial = false, FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });

            // Asignaturas
            await _database.InsertAsync(new Asignatura { Id = 1, Clave = "MAT101", Descripcion = "Cálculo Diferencial", Semestre = 1, FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });
            await _database.InsertAsync(new Asignatura { Id = 2, Clave = "FIS201", Descripcion = "Física II", Semestre = 3, FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });
            await _database.InsertAsync(new Asignatura { Id = 3, Clave = "PRG301", Descripcion = "Programación OO", Semestre = 3, FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });
            await _database.InsertAsync(new Asignatura { Id = 4, Clave = "BD401", Descripcion = "Base de Datos", Semestre = 4, FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });

            // Periodo semestral
            await _database.InsertAsync(new PeriodoSemestral { Id = 1, Descripcion = "ENERO - JULIO 2026", FechaInicia = new DateTime(2026, 1, 13), FechaTermina = new DateTime(2026, 7, 10), FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });

            // Tipos de adeudo
            await _database.InsertAsync(new TipoAdeudo { Id = 1, Descripcion = "Comprobante Vigencia IMSS", FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });
            await _database.InsertAsync(new TipoAdeudo { Id = 2, Descripcion = "Encuesta Institucional", FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });
            await _database.InsertAsync(new TipoAdeudo { Id = 3, Descripcion = "Acta de Nacimiento", FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });

            // ========== ALUMNOS DE PRUEBA ==========

            // Alumno 1: Sin problemas, con beca (FLUJO COMPLETO EXITOSO)
            await _database.InsertAsync(new Usuario { Id = 2, Nombre = "REYES VAZQUEZ SARAHI", Matricula = "I22050355", Clave = "123456", IdTipoUsuario = 2, IdBeca = 1, IdCarrera = 1, SemestreActual = 8, FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });

            // Alumno 2: Con materia especial pendiente (genera costo extra pero puede pagar)
            await _database.InsertAsync(new Usuario { Id = 3, Nombre = "GARCIA LOPEZ JUAN", Matricula = "I21030200", Clave = "123456", IdTipoUsuario = 2, IdBeca = null, IdCarrera = 2, SemestreActual = 6, FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });
            await _database.InsertAsync(new Oportunidad { Id = 1, IdUsuario = 3, IdAsignatura = 1, IdTipoOportunidad = 6, CostoExtra = 2500, Aprobado = null, FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });

            // Alumno 3: BLOQUEADO PERMANENTE (reprobó materia especial)
            await _database.InsertAsync(new Usuario { Id = 4, Nombre = "MARTINEZ RUIZ PEDRO", Matricula = "I20010100", Clave = "123456", IdTipoUsuario = 2, IdBeca = null, IdCarrera = 3, SemestreActual = 5, FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });
            await _database.InsertAsync(new Oportunidad { Id = 2, IdUsuario = 4, IdAsignatura = 2, IdTipoOportunidad = 6, CostoExtra = 2500, Aprobado = false, FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });

            // Alumno 4: BLOQUEADO POR DOCUMENTACIÓN
            await _database.InsertAsync(new Usuario { Id = 5, Nombre = "HERNANDEZ SOTO ANA", Matricula = "I23060400", Clave = "123456", IdTipoUsuario = 1, IdBeca = 2, IdCarrera = 1, SemestreActual = 2, FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });
            await _database.InsertAsync(new UsuarioTipoAdeudo { Id = 1, IdUsuario = 5, IdTipoAdeudo = 1, Observacion = "Entregar antes del 31 de marzo 2026", FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });
            await _database.InsertAsync(new UsuarioTipoAdeudo { Id = 2, IdUsuario = 5, IdTipoAdeudo = 2, Observacion = "Contestar en el portal antes del 20 de marzo", FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });

            // Alumno 5: Nuevo ingreso con beca 100% y materia global
            await _database.InsertAsync(new Usuario { Id = 6, Nombre = "TORRES DIAZ MARIA", Matricula = "I25070500", Clave = "123456", IdTipoUsuario = 1, IdBeca = 3, IdCarrera = 2, SemestreActual = 1, FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });
            await _database.InsertAsync(new Oportunidad { Id = 3, IdUsuario = 6, IdAsignatura = 3, IdTipoOportunidad = 8, CostoExtra = 1800, Aprobado = null, FechaCreacion = ahora, FechaModificacion = ahora, IdUsuarioCrea = 1 });
        }

        // --- Usuario ---
        public async Task<Usuario> ObtenerUsuarioPorCredenciales(string matricula, string clave)
        {
            return await _database.Table<Usuario>()
                .FirstOrDefaultAsync(u => u.Matricula == matricula && u.Clave == clave && u.Estatus);
        }

        public async Task<Usuario> ObtenerUsuarioPorId(int id)
        {
            return await _database.GetAsync<Usuario>(id);
        }

        // --- Bloqueos ---
        public async Task<List<Oportunidad>> ObtenerOportunidadesReprobadas(int idUsuario)
        {
            return await _database.Table<Oportunidad>()
                .Where(o => o.IdUsuario == idUsuario && o.Aprobado == false && o.Estatus)
                .ToListAsync();
        }

        public async Task<List<UsuarioTipoAdeudo>> ObtenerAdeudosActivos(int idUsuario)
        {
            return await _database.Table<UsuarioTipoAdeudo>()
                .Where(a => a.IdUsuario == idUsuario && a.Estatus)
                .ToListAsync();
        }

        public async Task<List<Oportunidad>> ObtenerOportunidadesPendientes(int idUsuario)
        {
            return await _database.Table<Oportunidad>()
                .Where(o => o.IdUsuario == idUsuario && o.Aprobado == null && o.Estatus)
                .ToListAsync();
        }

        // --- Catálogos ---
        public async Task<TipoOportunidad> ObtenerTipoOportunidad(int id) => await _database.GetAsync<TipoOportunidad>(id);
        public async Task<TipoAdeudo> ObtenerTipoAdeudo(int id) => await _database.GetAsync<TipoAdeudo>(id);
        public async Task<Beca> ObtenerBeca(int id) => await _database.GetAsync<Beca>(id);
        public async Task<TipoUsuario> ObtenerTipoUsuario(int id) => await _database.GetAsync<TipoUsuario>(id);
        public async Task<Carrera> ObtenerCarrera(int id) => await _database.GetAsync<Carrera>(id);
        public async Task<Asignatura> ObtenerAsignatura(int id) => await _database.GetAsync<Asignatura>(id);

        public async Task<Banco> ObtenerBancoPrincipal()
        {
            return await _database.Table<Banco>().FirstOrDefaultAsync(b => b.Estatus);
        }

        // --- Transaccion ---
        public async Task<int> GuardarTransaccion(Transaccion t)
        {
            await _database.InsertAsync(t);
            return t.Id;
        }

        public async Task ActualizarTransaccion(Transaccion t) => await _database.UpdateAsync(t);
        public async Task<Transaccion> ObtenerTransaccion(int id) => await _database.GetAsync<Transaccion>(id);

        // --- Periodo ---
        public async Task<PeriodoSemestral> ObtenerPeriodoActual()
        {
            var ahora = DateTime.Now;
            return await _database.Table<PeriodoSemestral>()
                .FirstOrDefaultAsync(p => p.FechaInicia <= ahora && p.FechaTermina >= ahora && p.Estatus);
        }

        // --- Inscripción ---
        public async Task GuardarInscripcion(UsuarioPeriodoSemestral i) => await _database.InsertAsync(i);
    }
}
