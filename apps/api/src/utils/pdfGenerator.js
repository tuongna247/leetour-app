/**
 * Receipt PDF Generator
 * Generates professional receipts for tour bookings
 *
 * Note: This implementation uses a simple HTML-to-PDF approach.
 * For production, consider using libraries like:
 * - @react-pdf/renderer (React-based)
 * - pdfkit (Node.js)
 * - puppeteer (Chrome headless)
 */

/**
 * Generate receipt HTML
 */
export function generateReceiptHTML(receipt, booking, tour, user) {
  const formattedDate = new Date(receipt.issueDate).toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  });

  const formatCurrency = (amount, currency = 'USD') => {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: currency
    }).format(amount);
  };

  return `
<!DOCTYPE html>
<html>
<head>
  <meta charset="UTF-8">
  <title>Receipt ${receipt.receiptNumber}</title>
  <style>
    * {
      margin: 0;
      padding: 0;
      box-sizing: border-box;
    }

    body {
      font-family: 'Helvetica', 'Arial', sans-serif;
      line-height: 1.6;
      color: #333;
      padding: 40px;
      max-width: 800px;
      margin: 0 auto;
    }

    .header {
      display: flex;
      justify-content: space-between;
      align-items: start;
      margin-bottom: 40px;
      border-bottom: 3px solid #2c3e50;
      padding-bottom: 20px;
    }

    .company-info {
      flex: 1;
    }

    .company-name {
      font-size: 28px;
      font-weight: bold;
      color: #2c3e50;
      margin-bottom: 5px;
    }

    .company-details {
      font-size: 12px;
      color: #666;
    }

    .receipt-info {
      text-align: right;
    }

    .receipt-title {
      font-size: 32px;
      font-weight: bold;
      color: #2c3e50;
      margin-bottom: 10px;
    }

    .receipt-number {
      font-size: 14px;
      color: #666;
      margin-bottom: 5px;
    }

    .receipt-date {
      font-size: 14px;
      color: #666;
    }

    .section {
      margin-bottom: 30px;
    }

    .section-title {
      font-size: 16px;
      font-weight: bold;
      color: #2c3e50;
      margin-bottom: 15px;
      padding-bottom: 5px;
      border-bottom: 2px solid #ecf0f1;
    }

    .info-grid {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 20px;
    }

    .info-block {
      margin-bottom: 10px;
    }

    .info-label {
      font-size: 12px;
      color: #7f8c8d;
      text-transform: uppercase;
      margin-bottom: 3px;
    }

    .info-value {
      font-size: 14px;
      color: #2c3e50;
      font-weight: 500;
    }

    table {
      width: 100%;
      border-collapse: collapse;
      margin-top: 10px;
    }

    thead {
      background-color: #2c3e50;
      color: white;
    }

    th, td {
      padding: 12px;
      text-align: left;
      border-bottom: 1px solid #ecf0f1;
    }

    th {
      font-size: 12px;
      text-transform: uppercase;
      font-weight: 600;
    }

    td {
      font-size: 14px;
    }

    .text-right {
      text-align: right;
    }

    .totals-section {
      margin-top: 30px;
      display: flex;
      justify-content: flex-end;
    }

    .totals-table {
      width: 350px;
    }

    .totals-table tr {
      border: none;
    }

    .totals-table td {
      padding: 8px 12px;
      border-bottom: 1px solid #ecf0f1;
    }

    .totals-table .total-row {
      font-weight: bold;
      font-size: 16px;
      background-color: #2c3e50;
      color: white;
    }

    .payment-status {
      display: inline-block;
      padding: 4px 12px;
      border-radius: 20px;
      font-size: 12px;
      font-weight: bold;
      text-transform: uppercase;
    }

    .payment-status.paid {
      background-color: #27ae60;
      color: white;
    }

    .payment-status.pending {
      background-color: #f39c12;
      color: white;
    }

    .footer {
      margin-top: 50px;
      padding-top: 20px;
      border-top: 2px solid #ecf0f1;
      text-align: center;
      font-size: 12px;
      color: #7f8c8d;
    }

    .notes {
      background-color: #ecf0f1;
      padding: 15px;
      border-radius: 5px;
      margin-top: 20px;
      font-size: 13px;
      color: #555;
    }

    @media print {
      body {
        padding: 20px;
      }
    }
  </style>
</head>
<body>
  <!-- Header -->
  <div class="header">
    <div class="company-info">
      <div class="company-name">LeeTour</div>
      <div class="company-details">
        Tour Booking & Travel Agency<br>
        Email: info@leetour.com<br>
        Phone: +1 (555) 123-4567<br>
        Website: www.leetour.com
      </div>
    </div>
    <div class="receipt-info">
      <div class="receipt-title">RECEIPT</div>
      <div class="receipt-number">Receipt #: ${receipt.receiptNumber}</div>
      <div class="receipt-date">Date: ${formattedDate}</div>
      <div style="margin-top: 10px;">
        <span class="payment-status ${receipt.paymentStatus}">${receipt.paymentStatus}</span>
      </div>
    </div>
  </div>

  <!-- Customer Information -->
  <div class="section">
    <div class="section-title">Customer Information</div>
    <div class="info-grid">
      <div>
        <div class="info-block">
          <div class="info-label">Name</div>
          <div class="info-value">${user.firstName} ${user.lastName}</div>
        </div>
        <div class="info-block">
          <div class="info-label">Email</div>
          <div class="info-value">${user.email}</div>
        </div>
      </div>
      <div>
        <div class="info-block">
          <div class="info-label">Booking Reference</div>
          <div class="info-value">${booking?.bookingReference || 'N/A'}</div>
        </div>
        <div class="info-block">
          <div class="info-label">Payment Method</div>
          <div class="info-value">${receipt.paymentMethod.replace('_', ' ').toUpperCase()}</div>
        </div>
      </div>
    </div>
  </div>

  <!-- Tour Information -->
  <div class="section">
    <div class="section-title">Tour Details</div>
    <div class="info-grid">
      <div>
        <div class="info-block">
          <div class="info-label">Tour Name</div>
          <div class="info-value">${tour?.title || 'N/A'}</div>
        </div>
      </div>
      <div>
        <div class="info-block">
          <div class="info-label">Tour Date</div>
          <div class="info-value">${booking?.tour?.selectedDate ? new Date(booking.tour.selectedDate).toLocaleDateString() : 'N/A'}</div>
        </div>
      </div>
    </div>
  </div>

  <!-- Items -->
  <div class="section">
    <div class="section-title">Items</div>
    <table>
      <thead>
        <tr>
          <th>Description</th>
          <th class="text-right">Quantity</th>
          <th class="text-right">Unit Price</th>
          <th class="text-right">Total</th>
        </tr>
      </thead>
      <tbody>
        ${receipt.items.map(item => `
          <tr>
            <td>${item.description}</td>
            <td class="text-right">${item.quantity}</td>
            <td class="text-right">${formatCurrency(item.unitPrice, receipt.currency)}</td>
            <td class="text-right">${formatCurrency(item.totalPrice, receipt.currency)}</td>
          </tr>
        `).join('')}
      </tbody>
    </table>
  </div>

  <!-- Totals -->
  <div class="totals-section">
    <table class="totals-table">
      <tr>
        <td>Subtotal:</td>
        <td class="text-right">${formatCurrency(receipt.subtotal, receipt.currency)}</td>
      </tr>
      ${receipt.surcharges > 0 ? `
        <tr>
          <td>Surcharges:</td>
          <td class="text-right">${formatCurrency(receipt.surcharges, receipt.currency)}</td>
        </tr>
      ` : ''}
      ${receipt.discounts > 0 ? `
        <tr>
          <td>Discounts:</td>
          <td class="text-right">-${formatCurrency(receipt.discounts, receipt.currency)}</td>
        </tr>
      ` : ''}
      ${receipt.tax.amount > 0 ? `
        <tr>
          <td>Tax (${receipt.tax.rate}%):</td>
          <td class="text-right">${formatCurrency(receipt.tax.amount, receipt.currency)}</td>
        </tr>
      ` : ''}
      <tr class="total-row">
        <td>TOTAL:</td>
        <td class="text-right">${formatCurrency(receipt.totalAmount, receipt.currency)}</td>
      </tr>
    </table>
  </div>

  ${receipt.notes ? `
    <div class="notes">
      <strong>Notes:</strong><br>
      ${receipt.notes}
    </div>
  ` : ''}

  <!-- Footer -->
  <div class="footer">
    <p>Thank you for choosing LeeTour!</p>
    <p>This is a computer-generated receipt and does not require a signature.</p>
    <p style="margin-top: 10px;">
      Generated with <a href="https://claude.com/claude-code" style="color: #3498db;">Claude Code</a>
    </p>
  </div>
</body>
</html>
  `;
}

/**
 * Generate receipt PDF using Puppeteer (server-side)
 * This is a placeholder - implement based on your chosen PDF library
 */
export async function generateReceiptPDF(receipt, booking, tour, user) {
  const html = generateReceiptHTML(receipt, booking, tour, user);

  // Option 1: Using Puppeteer (requires Chrome/Chromium)
  /*
  const puppeteer = require('puppeteer');
  const browser = await puppeteer.launch();
  const page = await browser.newPage();
  await page.setContent(html);
  const pdf = await page.pdf({
    format: 'A4',
    printBackground: true
  });
  await browser.close();
  return pdf;
  */

  // Option 2: Using html-pdf-node
  /*
  const htmlPdf = require('html-pdf-node');
  const options = { format: 'A4' };
  const file = { content: html };
  const pdfBuffer = await htmlPdf.generatePdf(file, options);
  return pdfBuffer;
  */

  // For now, return HTML (can be converted to PDF client-side or server-side)
  return html;
}

/**
 * Save receipt PDF to storage
 */
export async function saveReceiptPDF(receiptId, pdfBuffer) {
  // Implement based on your storage solution
  // - Save to local file system
  // - Upload to AWS S3
  // - Upload to Cloudinary
  // - Store in database as BLOB

  const filename = `receipt-${receiptId}.pdf`;
  const url = `/receipts/${filename}`;

  // TODO: Implement actual save logic
  return url;
}

export default {
  generateReceiptHTML,
  generateReceiptPDF,
  saveReceiptPDF
};
