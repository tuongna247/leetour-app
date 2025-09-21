
import {
    Typography, Box,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableRow,
    Chip
} from '@mui/material';
import DashboardCard from '@/app/components/shared/DashboardCard';

const tours = [
    {
        id: "1",
        name: "Amazing Bali Adventure",
        duration: "7 Days",
        location: "Bali, Indonesia",
        status: "Active",
        statusColor: "success.main",
        price: "1299",
    },
    {
        id: "2",
        name: "Paris City Tour",
        duration: "5 Days",
        location: "Paris, France",
        status: "Popular",
        statusColor: "primary.main",
        price: "1899",
    },
    {
        id: "3",
        name: "Tokyo Culture Experience",
        duration: "10 Days",
        location: "Tokyo, Japan",
        status: "Premium",
        statusColor: "warning.main",
        price: "2499",
    },
    {
        id: "4",
        name: "Maldives Beach Resort",
        duration: "6 Days",
        location: "Maldives",
        status: "Luxury",
        statusColor: "error.main",
        price: "3299",
    },
];


const ProductPerformance = () => {
    return (

        <DashboardCard title="Tour List">
            <Box sx={{ overflow: 'auto', width: { xs: '280px', sm: 'auto' } }}>
                <Table
                    aria-label="simple table"
                    sx={{
                        whiteSpace: "nowrap",
                        mt: 2
                    }}
                >
                    <TableHead>
                        <TableRow>
                            <TableCell>
                                <Typography variant="subtitle2" fontWeight={600}>
                                    Id
                                </Typography>
                            </TableCell>
                            <TableCell>
                                <Typography variant="subtitle2" fontWeight={600}>
                                    Tour Name
                                </Typography>
                            </TableCell>
                            <TableCell>
                                <Typography variant="subtitle2" fontWeight={600}>
                                    Location
                                </Typography>
                            </TableCell>
                            <TableCell>
                                <Typography variant="subtitle2" fontWeight={600}>
                                    Status
                                </Typography>
                            </TableCell>
                            <TableCell align="right">
                                <Typography variant="subtitle2" fontWeight={600}>
                                    Price
                                </Typography>
                            </TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {tours.map((tour) => (
                            <TableRow key={tour.name}>
                                <TableCell>
                                    <Typography
                                        sx={{
                                            fontSize: "15px",
                                            fontWeight: "500",
                                        }}
                                    >
                                        {tour.id}
                                    </Typography>
                                </TableCell>
                                <TableCell>
                                    <Box
                                        sx={{
                                            display: "flex",
                                            alignItems: "center",
                                        }}
                                    >
                                        <Box>
                                            <Typography variant="subtitle2" fontWeight={600}>
                                                {tour.name}
                                            </Typography>
                                            <Typography
                                                color="textSecondary"
                                                sx={{
                                                    fontSize: "13px",
                                                }}
                                            >
                                                {tour.duration}
                                            </Typography>
                                        </Box>
                                    </Box>
                                </TableCell>
                                <TableCell>
                                    <Typography color="textSecondary" variant="subtitle2" fontWeight={400}>
                                        {tour.location}
                                    </Typography>
                                </TableCell>
                                <TableCell>
                                    <Chip
                                        sx={{
                                            px: "4px",
                                            backgroundColor: tour.statusColor,
                                            color: "#fff",
                                        }}
                                        size="small"
                                        label={tour.status}
                                    ></Chip>
                                </TableCell>
                                <TableCell align="right">
                                    <Typography variant="h6">${tour.price}</Typography>
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </Box>
        </DashboardCard>
    );
};

export default ProductPerformance;
