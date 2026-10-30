using Microsoft.EntityFrameworkCore;
using MiroIntegration.Domain.Models;

namespace MiroIntegration.Infrastructure.Repositories;

public class PdfRepository(MiroDbContext db)
{
    public async Task<List<Pdf>> ListAsync(CancellationToken ct) => await db.Pdfs.ToListAsync(ct);
    public async Task<Pdf> CreateAsync(CancellationToken ct, Pdf entity) { db.Pdfs.Add(entity); await db.SaveChangesAsync(ct); return entity; }
    public async Task<Pdf?> GetByIdAsync(CancellationToken ct, Guid id) => await db.Pdfs.FirstOrDefaultAsync(x => x.Id == id, ct);
    public async Task<Pdf> UpdateAsync(CancellationToken ct, Pdf entity) { db.Pdfs.Update(entity); await db.SaveChangesAsync(ct); return entity; }
    public async Task DeleteAsync(CancellationToken ct, Guid id) => await db.Pdfs.Where(x => x.Id == id).ExecuteDeleteAsync(ct);
}
