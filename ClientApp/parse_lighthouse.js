import fs from 'fs';

const d = JSON.parse(fs.readFileSync('lighthouse-report.report.json', 'utf8'));
const p = d.categories.performance;
let out = 'Score: ' + (p.score * 100) + '\n\n=== Opportunities ===\n';

for (const key in d.audits) {
    const audit = d.audits[key];
    if (audit.details && audit.details.type === 'opportunity' && audit.details.overallSavingsMs > 0) {
        out += `- ${audit.title} (${audit.details.overallSavingsMs}ms)\n`;
        if (audit.details.items) {
            audit.details.items.forEach(item => {
                out += `  * ${item.url || (item.node && item.node.snippet) || ''} - Wasted: ${(item.wastedBytes / 1024).toFixed(2)}KB / ${item.wastedMs}ms\n`;
            });
        }
    }
}

out += '\n=== Metrics ===\n';
p.auditRefs.filter(r => r.weight > 0).forEach(r => {
    const a = d.audits[r.id];
    out += `${r.id}: ${a.displayValue} (Score: ${a.score}) / Weight: ${r.weight}\n`;
});

out += '\n=== Diagnostics ===\n';
p.auditRefs.filter(r => r.weight === 0).forEach(r => {
    const a = d.audits[r.id];
    if (a && a.score !== null && a.score < 1 && a.details && a.details.items && a.details.items.length) {
        out += `- ${r.id}: ${a.title}\n`;
        a.details.items.slice(0, 3).forEach(item => {
            out += `  ${item.url || (item.node && item.node.snippet) || JSON.stringify(item)}\n`;
        });
    }
});

fs.writeFileSync('parsed2.txt', out, 'utf8');
