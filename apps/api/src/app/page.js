export default function Home() {
  return (
    <div style={{ padding: '2rem', fontFamily: 'sans-serif' }}>
      <h1>LeeTour API</h1>
      <p>API Server is running on port 3001</p>
      <h2>Available Endpoints:</h2>
      <ul>
        <li><code>/api/auth/*</code> - Authentication endpoints</li>
        <li><code>/api/admin/*</code> - Admin endpoints</li>
        <li><code>/api/tours/*</code> - Tour endpoints</li>
        <li><code>/api/bookings/*</code> - Booking endpoints</li>
        <li><code>/api/payments/*</code> - Payment endpoints</li>
        <li><code>/api/v1/*</code> - API v1 endpoints</li>
      </ul>
    </div>
  )
}
