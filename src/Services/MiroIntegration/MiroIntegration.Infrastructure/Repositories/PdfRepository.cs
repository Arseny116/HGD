using Microsoft.EntityFrameworkCore;
using MiroIntegration.Application.Abstractions;
using MiroIntegration.Domain.Models;

namespace MiroIntegration.Infrastructure.Repositories;

public class PdfRepository(MiroDbContext db) : IPdfRepository
{
    public async Task AddAsync(Pdf pdf, CancellationToken cancellationToken)
    {
        db.Pdfs.Add(pdf);
        await db.SaveChangesAsync(cancellationToken);
    }

    public Task<Pdf?> FindByIdAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken) => db.Pdfs
        .Include(pdf => pdf.Project)
        .FirstOrDefaultAsync(
            pdf => pdf.Id == id && pdf.Project.UserId == userId,
            cancellationToken);

    public Task<Pdf?> FindByJobIdAsync(
        Guid jobId,
        Guid userId,
        CancellationToken cancellationToken) => db.Pdfs
        .Include(pdf => pdf.Project)
        .FirstOrDefaultAsync(
            pdf => pdf.JobId == jobId && pdf.Project.UserId == userId,
            cancellationToken);

    public Task<List<Pdf>> ListByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken) => db.Pdfs
        .Where(pdf => pdf.Project.UserId == userId)
        .OrderByDescending(pdf => pdf.Id)
        .ToListAsync(cancellationToken);

    public Task<List<Pdf>> ListByProjectIdAsync(
        Guid projectId,
        Guid userId,
        CancellationToken cancellationToken) => db.Pdfs
        .Where(pdf => pdf.ProjectId == projectId && pdf.Project.UserId == userId)
        .OrderByDescending(pdf => pdf.Id)
        .ToListAsync(cancellationToken);

    public async Task UpdateAsync(Pdf pdf, CancellationToken cancellationToken)
    {
        db.Pdfs.Update(pdf);
        await db.SaveChangesAsync(cancellationToken);
    }
}
