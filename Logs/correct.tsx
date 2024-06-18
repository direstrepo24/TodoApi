import '../../assets/styles/pages/paymentOption.scss';
import imagePayout from '@presentation/assets/images/img-payout-values.png';
import iconArrow from '@presentation/assets/icons/icon_arrow.png';
import HeaderMessagePayment from '@presentation/components/atomic/molecules/HeaderMessagePayment';
import ICardContainerTextV from '@presentation/components/atomic/atoms/WhiteCardContainer/OnlyTextV';
import ICardContainerTextH from '@presentation/components/atomic/atoms/WhiteCardContainer/OnlyTextH';
import InformationModal from '@presentation/components/atomic/molecules/Modals/AlertModal';
import ICardInformation from '@presentation/components/atomic/atoms/InformationCard';
import Footer from '@presentation/components/atomic/molecules/Footer';
import ButtonPayment from '@presentation/components/atomic/molecules/ButtonPayment';
import $ from 'jquery';
import { ButtonType, IsButtonBancolombia, IsButtonPSE, IsCastigado, StatePaymentName, StatePaymentValue } from '@presentation/store/data-store';
import { useRecoilValue, useSetRecoilState } from 'recoil';
import { StateManagement } from '@presentation/hooks/states-managements';
import { useNavigate } from 'react-router-dom';
import { ROUTE_BANK_SELECT, ROUTE_PAYOUT_VALUES, ROUTE_PUNISHED_PORTAL } from '@presentation/routes/AppRoutes';
import { CreatePaymentUseCase, CreatePaymentPunishedUseCase } from '@application/users/commands';
import { di } from '@di/app.container';
import { USER_SYMBOLS, ValuesCreatePaymentsResponse } from '@domain/users';
import { Failure } from '@core/index';
import { apiRequestManagements } from '@presentation/hooks/api-request-managements';
import { Fragment } from 'react/jsx-runtime';

const PaymentOption = () => {
    const createPayment = di.get<CreatePaymentUseCase>(USER_SYMBOLS.USER_CREATE_PAYMENT);
    const createPaymentPunished = di.get<CreatePaymentPunishedUseCase>(USER_SYMBOLS.USER_CREATE_PAYMENT_PUNISHED);
    const getStatePaymentValue = useRecoilValue(StatePaymentValue);
    const getStatePaymentName = useRecoilValue(StatePaymentName);
    const getIsCastigado = useRecoilValue(IsCastigado);
    const getIsButtonPSE = useRecoilValue(IsButtonPSE);
    const getIsButtonBancolombia = useRecoilValue(IsButtonBancolombia);
    const setButtonType = useSetRecoilState(ButtonType);
    const navigate = useNavigate();
    const {
        setIsDisabled,
        setModalType,
        toggleModal,
        isDisabled,
        isModalActivate,
        modalType
    } = StateManagement();
    const {
        requestCreatePaymentBancolombia,
        requestCreatePaymentBancolombiaPunished
    } = apiRequestManagements();

    const returnBack = () => {
        navigate(getIsCastigado ? ROUTE_PUNISHED_PORTAL : ROUTE_PAYOUT_VALUES, { replace: true });
    }

    const handleErrorResponse = (error) => {
        const errorMap = {
            "400": "NotMatch",
            "401": "Inactivity",
            "404": "NotMatch",
            "405": "ErrorACH",
            "406": "Pending",
            "416": "ExceedsLimit"
        };
        setModalType(errorMap[error.key] || "SystemError");
        toggleModal();
    };

    const handlerOnSubmit = async (name) => {
        $(".card-container-payment").css({ opacity: ".5", border: "transparent" });
        if (name === "BOTONBANCOLOMBIA") {
            setIsDisabled(true);
            setButtonType(2);
            try {
                const result = getIsCastigado ? await createPaymentPunished.execute(requestCreatePaymentBancolombiaPunished) : await createPayment.execute(requestCreatePaymentBancolombia);
                result.fold(
                    (data: ValuesCreatePaymentsResponse) => {
                        window.location.replace(data.redirectUrl);
                    },
                    handleErrorResponse
                );
            } catch (error) {
                setModalType("SystemError");
                toggleModal();
            }
        } else if (name === "PSE") {
            setButtonType(3);
            navigate(ROUTE_BANK_SELECT, { replace: true });
        } else {
            console.error('error: ' + name);
        }
    };

    return (
        <>
            <div className='header-container-mobile'>
                <HeaderMessagePayment id='desk-HeaderMessagePayment' />
                <img src={imagePayout} className='img-home' alt="Not Found" />
            </div>
            {/* Remaining component code */}
        </>
    )
}

export default PaymentOption;
