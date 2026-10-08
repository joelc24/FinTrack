
using FinTrack.Domain.ExpenseCategories;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Persistence.Seeding;

public record SubCategoriesSeed(string Title, string Description);
public record CategorySeed(string Title, string Description, List<SubCategoriesSeed> SubCategories);

public static class ExpenseCategorySeeder 
{
    public static async Task SeedAsync(AppDbContext dbContext, CancellationToken ct = default)
    {
        if(await dbContext.ExpenseCategories.AnyAsync(ct))
            return;

        var seedData = new List<CategorySeed>
        {
            new("Gastos Fijos Esenciales", "Son los pagos obligatorios y recurrentes que debes hacer cada mes para subsistir. De no pagarse, generan consecuencias graves o deudas", [
                new("Vivienda", "Pago de alquiler o cuota de la hipoteca."),
                new("Servicios publicos", "Facturas de electricidad, gas, agua, internet y telefonía movil."),
                new("Alimentacion Basica", "Las compras del mercado y supermercado para el hogar."),
                new("Salud y seguros", "Medicamentos, polizas de salud, seguros de vivienda o del vehiculo."),
                new("Transporte", "Combustible, pasajes de transporte publico o cuotas de creditos vehiculares."),
                new("Educacion", "Colegiaturas, matriculas o guarderias.")
            ]),
            new(
                "Gastos Variables Necesarios", 
                "Son aquellos que cambian de monto cada mes segun el consumo o las necesidades puntuales, pero siguen siendo importantes.", [
                    new("Mantenimiento y reparaciones", "Arreglos imprevistos en casa o reparaciones del auto."),
                    new("Cuidado personal", "Compra de ropa, articulos de aseo personal o visitas a la peluqueria."),
                    new("Mascotas", "Alimentos y veterinario para animales de compania.")
            ]),

            new(
                "Ahorro e Inversion", 
                "La porcion del ingreso que se aparta antes de gastar en otros rubros.",
                [
                    new("Fondo de emergencia", "Dinero reservado para imprevistos medicos o perdida de empleo."),
                    new("Ahorro para metas", "Fondos destinados a educacion futura, viajes o la inicial de una vivienda.")
                ]
            ),

            new(
                "Ocio, Entretenimiento y Gastos Discrecionales", 
                "Son los gastos opcionales orientados al bienestar y la recreacion familiar.",
                [
                    new("Salidas y diversion", "Ir a restaurantes, cines, parques o pedir comida a domicilio."),
                    new("Suscripciones", "Plataformas de entretenimiento o streaming.")
                ]
            ),

            new(
                "Gastos Hormiga o Invisibles", 
                "Pequenos consumos cotidianos que pasan desapercibidos pero afectan el presupuesto al sumarlos mes a mes.",
                [
                    new("Cafes diarios, snacks o golosinas", "Gastos pequenos en transporte informal o compras impulsivas.")
                ]
            )
        };

        foreach (var categoryData in seedData)
        {
            var categoryResult = ExpenseCategory.Create(categoryData.Title, categoryData.Description);
            if(categoryResult.IsFailure) continue;

            var category = categoryResult.Value;
            foreach (var subCategoryData in categoryData.SubCategories)
            {
                category.AddSubCategory(subCategoryData.Title, subCategoryData.Description);
            }

            dbContext.ExpenseCategories.Add(category);
        }

        await dbContext.SaveChangesAsync(ct);
    }   
}