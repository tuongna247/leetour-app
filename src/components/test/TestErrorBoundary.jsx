import React from 'react';
import {
  Box,
  Container,
  Typography,
  Button,
  Paper,
  Alert,
  AlertTitle,
  Stack,
  Divider
} from '@mui/material';
import {
  Refresh as RefreshIcon,
  Home as HomeIcon,
  BugReport as BugIcon
} from '@mui/icons-material';
import ErrorDisplay from '../ui/ErrorDisplay';

class TestErrorBoundary extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      hasError: false,
      error: null,
      errorInfo: null,
      errorId: null
    };
  }

  static getDerivedStateFromError(error) {
    // Update state so the next render will show the fallback UI
    return {
      hasError: true,
      error: error,
      errorId: Date.now().toString()
    };
  }

  componentDidCatch(error, errorInfo) {
    // Log error details
    console.error('Test Error Boundary caught an error:', error, errorInfo);
    
    this.setState({
      error: error,
      errorInfo: errorInfo
    });

    // Send error to monitoring service if available
    if (typeof window !== 'undefined' && window.gtag) {
      window.gtag('event', 'exception', {
        description: error.toString(),
        fatal: false
      });
    }
  }

  handleRetry = () => {
    this.setState({
      hasError: false,
      error: null,
      errorInfo: null,
      errorId: null
    });
  };

  handleGoHome = () => {
    if (typeof window !== 'undefined') {
      window.location.href = '/';
    }
  };

  render() {
    if (this.state.hasError) {
      return (
        <Container maxWidth="lg" sx={{ py: 4 }}>
          <Paper elevation={3} sx={{ p: 4 }}>
            <Stack spacing={4}>
              {/* Header */}
              <Box textAlign="center">
                <BugIcon sx={{ fontSize: 64, color: 'error.main', mb: 2 }} />
                <Typography variant="h4" gutterBottom color="error.main">
                  Test Error Detected
                </Typography>
                <Typography variant="body1" color="text.secondary">
                  An error occurred during testing. Please review the details below.
                </Typography>
              </Box>

              <Divider />

              {/* Error Details */}
              <ErrorDisplay
                error={this.state.error}
                title="Application Error"
                severity="error"
                showDetails={true}
                onRetry={this.handleRetry}
                collapsible={true}
              />

              {/* Stack Trace (Development) */}
              {process.env.NODE_ENV === 'development' && this.state.errorInfo && (
                <Box>
                  <Typography variant="h6" gutterBottom color="error.main">
                    Component Stack Trace:
                  </Typography>
                  <Paper 
                    variant="outlined" 
                    sx={{ 
                      p: 2, 
                      backgroundColor: 'grey.50',
                      fontFamily: 'monospace',
                      fontSize: '0.75rem',
                      overflow: 'auto',
                      maxHeight: '200px'
                    }}
                  >
                    <pre>{this.state.errorInfo.componentStack}</pre>
                  </Paper>
                </Box>
              )}

              {/* Error ID for Support */}
              {this.state.errorId && (
                <Alert severity="info" variant="outlined">
                  <AlertTitle>Error Reference ID</AlertTitle>
                  Please reference this ID when reporting the issue: <strong>{this.state.errorId}</strong>
                </Alert>
              )}

              {/* Action Buttons */}
              <Box display="flex" justifyContent="center" gap={2}>
                <Button
                  variant="contained"
                  color="primary"
                  startIcon={<RefreshIcon />}
                  onClick={this.handleRetry}
                  size="large"
                >
                  Try Again
                </Button>
                
                <Button
                  variant="outlined"
                  color="secondary"
                  startIcon={<HomeIcon />}
                  onClick={this.handleGoHome}
                  size="large"
                >
                  Go Home
                </Button>
              </Box>

              {/* Development Tools */}
              {process.env.NODE_ENV === 'development' && (
                <Box>
                  <Divider sx={{ my: 2 }} />
                  <Typography variant="h6" gutterBottom>
                    Development Tools
                  </Typography>
                  <Stack direction="row" spacing={2}>
                    <Button
                      variant="outlined"
                      size="small"
                      onClick={() => console.log('Error:', this.state.error)}
                    >
                      Log to Console
                    </Button>
                    <Button
                      variant="outlined"
                      size="small"
                      onClick={() => {
                        const errorData = {
                          error: this.state.error?.toString(),
                          stack: this.state.error?.stack,
                          componentStack: this.state.errorInfo?.componentStack,
                          timestamp: new Date().toISOString()
                        };
                        navigator.clipboard.writeText(JSON.stringify(errorData, null, 2));
                      }}
                    >
                      Copy Error Data
                    </Button>
                  </Stack>
                </Box>
              )}
            </Stack>
          </Paper>
        </Container>
      );
    }

    return this.props.children;
  }
}

export default TestErrorBoundary;