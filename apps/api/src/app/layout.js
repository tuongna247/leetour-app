export const metadata = {
  title: 'LeeTour API',
  description: 'LeeTour API Server',
}

export default function RootLayout({ children }) {
  return (
    <html lang="en">
      <body>{children}</body>
    </html>
  )
}
