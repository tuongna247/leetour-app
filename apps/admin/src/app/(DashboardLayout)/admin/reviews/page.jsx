'use client';

import React, { useState, useEffect } from 'react';
import {
  Box,
  Card,
  CardContent,
  Typography,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  TablePagination,
  Paper,
  Chip,
  IconButton,
  Button,
  Stack,
  Rating,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Tabs,
  Tab,
  Alert,
  Avatar
} from '@mui/material';
import {
  CheckCircle as ApproveIcon,
  Cancel as RejectIcon,
  Delete as DeleteIcon,
  Visibility as ViewIcon,
  VerifiedUser as VerifiedIcon
} from '@mui/icons-material';

export default function ReviewModerationPage() {
  const [reviews, setReviews] = useState([]);
  const [loading, setLoading] = useState(true);
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const [totalReviews, setTotalReviews] = useState(0);
  const [statusFilter, setStatusFilter] = useState('pending');
  const [selectedReview, setSelectedReview] = useState(null);
  const [dialogOpen, setDialogOpen] = useState(false);
  const [adminNotes, setAdminNotes] = useState('');
  const [statistics, setStatistics] = useState({
    pending: 0,
    approved: 0,
    rejected: 0,
    total: 0
  });

  useEffect(() => {
    fetchReviews();
  }, [page, rowsPerPage, statusFilter]);

  const fetchReviews = async () => {
    setLoading(true);
    try {
      const params = new URLSearchParams({
        page: page + 1,
        limit: rowsPerPage,
        status: statusFilter
      });

      const response = await fetch(`/api/admin/reviews?${params}`);
      const data = await response.json();

      if (data.success) {
        setReviews(data.data.reviews);
        setTotalReviews(data.data.pagination.totalReviews);
        setStatistics(data.data.statistics);
      }
    } catch (error) {
      console.error('Error fetching reviews:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleApprove = async (reviewId) => {
    try {
      const response = await fetch(`/api/admin/reviews/${reviewId}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ status: 'approved' })
      });

      if (response.ok) {
        fetchReviews();
        setDialogOpen(false);
      }
    } catch (error) {
      console.error('Error approving review:', error);
      alert('Failed to approve review');
    }
  };

  const handleReject = async (reviewId, notes) => {
    try {
      const response = await fetch(`/api/admin/reviews/${reviewId}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          status: 'rejected',
          adminNotes: notes
        })
      });

      if (response.ok) {
        fetchReviews();
        setDialogOpen(false);
        setAdminNotes('');
      }
    } catch (error) {
      console.error('Error rejecting review:', error);
      alert('Failed to reject review');
    }
  };

  const handleDelete = async (reviewId) => {
    if (!confirm('Are you sure you want to permanently delete this review?')) return;

    try {
      const response = await fetch(`/api/admin/reviews/${reviewId}`, {
        method: 'DELETE'
      });

      if (response.ok) {
        fetchReviews();
      }
    } catch (error) {
      console.error('Error deleting review:', error);
      alert('Failed to delete review');
    }
  };

  const handleViewDetails = (review) => {
    setSelectedReview(review);
    setAdminNotes(review.adminNotes || '');
    setDialogOpen(true);
  };

  const getStatusColor = (status) => {
    switch (status) {
      case 'approved': return 'success';
      case 'rejected': return 'error';
      case 'pending': return 'warning';
      default: return 'default';
    }
  };

  const formatDate = (date) => {
    return new Date(date).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  };

  return (
    <Box sx={{ p: 3 }}>
      <Typography variant="h4" gutterBottom>
        Review Moderation
      </Typography>

      {/* Statistics */}
      <Stack direction="row" spacing={2} sx={{ mb: 3 }}>
        <Card sx={{ flex: 1 }}>
          <CardContent>
            <Typography color="text.secondary" gutterBottom>
              Pending
            </Typography>
            <Typography variant="h4">{statistics.pending}</Typography>
          </CardContent>
        </Card>
        <Card sx={{ flex: 1 }}>
          <CardContent>
            <Typography color="text.secondary" gutterBottom>
              Approved
            </Typography>
            <Typography variant="h4">{statistics.approved}</Typography>
          </CardContent>
        </Card>
        <Card sx={{ flex: 1 }}>
          <CardContent>
            <Typography color="text.secondary" gutterBottom>
              Rejected
            </Typography>
            <Typography variant="h4">{statistics.rejected}</Typography>
          </CardContent>
        </Card>
        <Card sx={{ flex: 1 }}>
          <CardContent>
            <Typography color="text.secondary" gutterBottom>
              Total Reviews
            </Typography>
            <Typography variant="h4">{statistics.total}</Typography>
          </CardContent>
        </Card>
      </Stack>

      {/* Filter Tabs */}
      <Card sx={{ mb: 2 }}>
        <Tabs
          value={statusFilter}
          onChange={(e, newValue) => {
            setStatusFilter(newValue);
            setPage(0);
          }}
          sx={{ borderBottom: 1, borderColor: 'divider' }}
        >
          <Tab label={`Pending (${statistics.pending})`} value="pending" />
          <Tab label={`Approved (${statistics.approved})`} value="approved" />
          <Tab label={`Rejected (${statistics.rejected})`} value="rejected" />
          <Tab label="All" value="" />
        </Tabs>
      </Card>

      {/* Reviews Table */}
      <Card>
        <TableContainer>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>Tour</TableCell>
                <TableCell>User</TableCell>
                <TableCell>Rating</TableCell>
                <TableCell>Review</TableCell>
                <TableCell>Date</TableCell>
                <TableCell>Status</TableCell>
                <TableCell>reCAPTCHA</TableCell>
                <TableCell align="right">Actions</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {reviews.map((review) => (
                <TableRow key={review._id}>
                  <TableCell>
                    <Typography variant="body2" fontWeight="medium">
                      {review.tour?.title || 'N/A'}
                    </Typography>
                  </TableCell>
                  <TableCell>
                    <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                      <Avatar sx={{ width: 32, height: 32 }}>
                        {review.user?.firstName?.[0]}
                      </Avatar>
                      <Box>
                        <Typography variant="body2">
                          {review.user?.firstName} {review.user?.lastName}
                        </Typography>
                        {review.verifiedPurchase && (
                          <Chip
                            icon={<VerifiedIcon />}
                            label="Verified"
                            size="small"
                            color="primary"
                            variant="outlined"
                          />
                        )}
                      </Box>
                    </Box>
                  </TableCell>
                  <TableCell>
                    <Rating value={review.rating} readOnly size="small" />
                  </TableCell>
                  <TableCell>
                    <Typography variant="body2" fontWeight="medium">
                      {review.title}
                    </Typography>
                    <Typography variant="caption" color="text.secondary" noWrap sx={{ maxWidth: 300, display: 'block' }}>
                      {review.comment.substring(0, 100)}...
                    </Typography>
                  </TableCell>
                  <TableCell>
                    <Typography variant="caption">
                      {formatDate(review.createdAt)}
                    </Typography>
                  </TableCell>
                  <TableCell>
                    <Chip
                      label={review.status}
                      size="small"
                      color={getStatusColor(review.status)}
                    />
                  </TableCell>
                  <TableCell>
                    <Chip
                      label={review.recaptchaScore?.toFixed(2) || 'N/A'}
                      size="small"
                      color={review.recaptchaScore >= 0.5 ? 'success' : 'error'}
                      variant="outlined"
                    />
                  </TableCell>
                  <TableCell align="right">
                    <Stack direction="row" spacing={1} justifyContent="flex-end">
                      <IconButton
                        size="small"
                        onClick={() => handleViewDetails(review)}
                      >
                        <ViewIcon fontSize="small" />
                      </IconButton>

                      {review.status === 'pending' && (
                        <>
                          <IconButton
                            size="small"
                            color="success"
                            onClick={() => handleApprove(review._id)}
                          >
                            <ApproveIcon fontSize="small" />
                          </IconButton>
                          <IconButton
                            size="small"
                            color="error"
                            onClick={() => handleViewDetails(review)}
                          >
                            <RejectIcon fontSize="small" />
                          </IconButton>
                        </>
                      )}

                      <IconButton
                        size="small"
                        color="error"
                        onClick={() => handleDelete(review._id)}
                      >
                        <DeleteIcon fontSize="small" />
                      </IconButton>
                    </Stack>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>

        <TablePagination
          component="div"
          count={totalReviews}
          page={page}
          onPageChange={(e, newPage) => setPage(newPage)}
          rowsPerPage={rowsPerPage}
          onRowsPerPageChange={(e) => {
            setRowsPerPage(parseInt(e.target.value, 10));
            setPage(0);
          }}
        />
      </Card>

      {/* Review Details Dialog */}
      <Dialog open={dialogOpen} onClose={() => setDialogOpen(false)} maxWidth="md" fullWidth>
        {selectedReview && (
          <>
            <DialogTitle>
              Review Details
              <Chip
                label={selectedReview.status}
                size="small"
                color={getStatusColor(selectedReview.status)}
                sx={{ ml: 2 }}
              />
            </DialogTitle>
            <DialogContent dividers>
              <Stack spacing={2}>
                <Box>
                  <Typography variant="subtitle2" color="text.secondary">
                    Tour
                  </Typography>
                  <Typography>{selectedReview.tour?.title}</Typography>
                </Box>

                <Box>
                  <Typography variant="subtitle2" color="text.secondary">
                    User
                  </Typography>
                  <Typography>
                    {selectedReview.user?.firstName} {selectedReview.user?.lastName}
                    {selectedReview.verifiedPurchase && (
                      <Chip icon={<VerifiedIcon />} label="Verified Purchase" size="small" sx={{ ml: 1 }} />
                    )}
                  </Typography>
                  <Typography variant="caption" color="text.secondary">
                    {selectedReview.user?.email}
                  </Typography>
                </Box>

                <Box>
                  <Typography variant="subtitle2" color="text.secondary">
                    Rating
                  </Typography>
                  <Rating value={selectedReview.rating} readOnly />
                </Box>

                <Box>
                  <Typography variant="subtitle2" color="text.secondary">
                    Title
                  </Typography>
                  <Typography>{selectedReview.title}</Typography>
                </Box>

                <Box>
                  <Typography variant="subtitle2" color="text.secondary">
                    Comment
                  </Typography>
                  <Typography>{selectedReview.comment}</Typography>
                </Box>

                <Box>
                  <Typography variant="subtitle2" color="text.secondary">
                    reCAPTCHA Score
                  </Typography>
                  <Chip
                    label={selectedReview.recaptchaScore?.toFixed(2) || 'N/A'}
                    color={selectedReview.recaptchaScore >= 0.5 ? 'success' : 'error'}
                  />
                </Box>

                <Box>
                  <Typography variant="subtitle2" color="text.secondary">
                    IP Address
                  </Typography>
                  <Typography variant="caption">{selectedReview.ipAddress || 'N/A'}</Typography>
                </Box>

                {selectedReview.status === 'pending' && (
                  <TextField
                    fullWidth
                    multiline
                    rows={3}
                    label="Admin Notes (for rejection)"
                    value={adminNotes}
                    onChange={(e) => setAdminNotes(e.target.value)}
                    placeholder="Optional: Reason for rejection..."
                  />
                )}

                {selectedReview.adminNotes && (
                  <Alert severity="info">
                    <Typography variant="subtitle2">Admin Notes:</Typography>
                    <Typography variant="body2">{selectedReview.adminNotes}</Typography>
                  </Alert>
                )}
              </Stack>
            </DialogContent>
            <DialogActions>
              <Button onClick={() => setDialogOpen(false)}>
                Close
              </Button>
              {selectedReview.status === 'pending' && (
                <>
                  <Button
                    onClick={() => handleReject(selectedReview._id, adminNotes)}
                    color="error"
                    variant="outlined"
                  >
                    Reject
                  </Button>
                  <Button
                    onClick={() => handleApprove(selectedReview._id)}
                    color="success"
                    variant="contained"
                  >
                    Approve
                  </Button>
                </>
              )}
            </DialogActions>
          </>
        )}
      </Dialog>
    </Box>
  );
}
