import { useNavigate, useParams } from 'react-router-dom';
import WelcomeBand from '../components/WelcomeBand';
import { useCart } from '../context/CartContext';
import { CartItem } from '../types/CartItem';
import { useState } from 'react';

function ConfirmationPage() {
  const navigate = useNavigate();
  const { bookTitle, bookPrice, bookId } = useParams();
  const { addToCart } = useCart();
  const [bookQuantity, setBookQuantity] = useState<number>(1);

  const handleAddToCart = () => {
    const newItem: CartItem = {
      bookId: Number(bookId),
      bookTitle: bookTitle || 'No Project Found',
      bookPrice: Number(bookPrice),
      bookQuantity,
    };
    addToCart(newItem);
    navigate('/cart');
  };

  return (
    <>
      <WelcomeBand />
      <h2>How many would you like?</h2>
      <div>
        <input
          type="number"
          placeholder="Enter Quantity"
          value={bookQuantity}
          onChange={(x) => setBookQuantity(Number(x.target.value))}
        />
        <br />
        <button onClick={handleAddToCart}>Add {bookTitle} to Cart</button>
      </div>
      <button onClick={() => navigate('/')}>Go Back</button>
    </>
  );
}

export default ConfirmationPage;
