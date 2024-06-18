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
import { CreatePaymentUseCase } from '@application/users/commands/createPayment.usecase';
import { di } from '@di/app.container';
import { USER_SYMBOLS, ValuesCreatePaymentsResponse } from '@domain/users';
import { Failure } from '@core/index';
import { apiRequestManagements } from '@presentation/hooks/api-request-managements';
import { CreatePaymentPunishedUseCase } from '@application/users/commands/createPaymentPunished.usecase';
import { Fragment } from 'react/jsx-runtime';
const PaymentOption = () => {
    //Metodo para generar creación de pago cuando vienen por Bancolombia
    const createPayment = di.get<CreatePaymentUseCase>(USER_SYMBOLS.USER_CREATE_PAYMENT);
    //Metodo para generar creación de pago cuando vienen por Bancolombia (Castigo)
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
        if (getIsCastigado) {
            navigate(ROUTE_PUNISHED_PORTAL, { replace: true });
        } else {
            navigate(ROUTE_PAYOUT_VALUES, { replace: true });
        }
    }
    const handlerOnSubmit = async (name: string) => {
        $(".card-container-payment").css("opacity", ".5");
        $(".card-container-payment").css("border", "transparent");
        if (name == "BOTONBANCOLOMBIA") {
            setIsDisabled(true);
            setButtonType(2);
            if (getIsCastigado) {
                try {
                    const result = await createPaymentPunished.execute(requestCreatePaymentBancolombiaPunished);
                    result.fold((data: ValuesCreatePaymentsResponse) => {
                        window.location.replace(data.redirectUrl);
                    }, (_: Failure) => {
                        switch (_.key) {
                            case "400":
                                setModalType("NotMatch");
                                break;
                            case "401":
                                setModalType("Inactivity");
                                break;
                            case "404":
                                setModalType("NotMatch");
                                break;
                            case "405":
                                setModalType("ErrorACH");
                                break;
                            case "406":
                                setModalType("Pending");
                                break;
                            case "416":
                                setModalType("ExceedsLimit");
                                break;
                            default:
                                setModalType("SystemError");
                        }
                        toggleModal();
                    });
                } catch (error) {
                    setModalType("SystemError");
                    toggleModal();
                }
            } else {
                try {
                    const result = await createPayment.execute(requestCreatePaymentBancolombia);
                    result.fold((data: ValuesCreatePaymentsResponse) => {
                        window.location.replace(data.redirectUrl);
                    }, (_: Failure) => {
                        switch (_.key) {
                            case "400":
                                setModalType("NotMatch");
                                break;
                            case "401":
                                setModalType("Inactivity");
                                break;
                            case "404":
                                setModalType("NotMatch");
                                break;
                            case "405":
                                setModalType("ErrorACH");
                                break;
                            case "406":
                                setModalType("Pending");
                                break;
                            case "416":
                                setModalType("ExceedsLimit");
                                break;
                            default:
                                setModalType("SystemError");
                        }
                        toggleModal();
                    });
                } catch (error) {
                    setModalType("SystemError");
                    toggleModal();
                }
            }
        } else if (name == "PSE") {
            setButtonType(3)
            navigate(ROUTE_BANK_SELECT, { replace: true });
        } else {
            console.log('error: ' + name);
        }
    }
    return (
        <>
            <div className='header-container-mobile'>
                <HeaderMessagePayment
                    id='desk-HeaderMessagePayment' />
                <img src={imagePayout} className='img-home' alt="Not Found" />
            </div>
            <div className='home-page-container home-page-container-po flex flex-wrap content-center justify-around flex-col'>
                <div className='header-container grid grid'>
                    <HeaderMessagePayment
                        id='desk-HeaderMessagePayment' />
                    <img src={imagePayout} className='img-home' alt="Not Found" />
                </div>
                <div className='form-container form-container-po grid'>
                    <div className='return-back' onClick={() => returnBack()}>
                        <img src={iconArrow} className='img-arrow' alt="Not Found" />
                        <p>Volver</p>
                    </div>
                    <div className='form-message-text'>Detalle de tu pago</div>
                    <div className='container-cards-vertical flex'>
                        <ICardContainerTextV
                            id='card-container-text-POpt-first'
                            idText='text-ot-normal'
                            title={'Tipo de pago'}
                            text={getStatePaymentName} />
                        <ICardContainerTextV
                            id='card-container-text-POpt'
                            title={'Valor de pago'}
                            text={'$' + new Intl.NumberFormat('de-DE').format(getStatePaymentValue)} />
                    </div>
                    <div className='container-cards-horizontal grid'>
                        <ICardContainerTextH
                            id='card-container-text-POpt-first'
                            idText='text-ot-normal'
                            title={'Tipo de pago'}
                            text={getStatePaymentName} />
                        <ICardContainerTextH
                            id='card-container-text-POpt'
                            title={'Valor de pago'}
                            text={'$' + new Intl.NumberFormat('de-DE').format(getStatePaymentValue)} />
                    </div>
                    <div className='form-message-text form-message-text-po'>Selecciona tu medio de pago</div>
                    {!getIsButtonBancolombia ? (
                        <Fragment>
                            <ButtonPayment
                                disabled={true}
                                title={'Botón Bancolombia'}
                                text={'Desde tu cuenta Bancolombia'}
                                imgName={'imgBancolombia'} />
                            <ICardInformation
                                textTitle='Lo sentimos:'
                                firstTextMsg='El botón bancolombia no esta disponible en estos momentos, inténtalo mas tarde.'
                                afterStrongTextMsg='También '
                                strongTextMsg='puedes pagar a través del botón PSE'
                                secondTextMsg='de forma ágil y segura.' />
                        </Fragment>
                    ) : (
                        <ButtonPayment
                            disabled={!isDisabled}
                            title={'Botón Bancolombia'}
                            text={'Desde tu cuenta Bancolombia'}
                            imgName={'imgBancolombia'}
                            onClick={() => handlerOnSubmit("BOTONBANCOLOMBIA")} />)
                    }
                    {!getIsButtonPSE ? (
                        <Fragment>
                            <ButtonPayment
                                disabled={true}
                                title={'Botón PSE'}
                                text={'Desde tu cuenta de ahorros o corriente'}
                                imgName={'imgPSE'} />
                            <ICardInformation
                                textTitle='Lo sentimos:'
                                firstTextMsg='El botón PSE no esta disponible en estos momentos, inténtalo mas tarde.'
                                afterStrongTextMsg='También '
                                strongTextMsg='puedes pagar a través del botón Bancolombia'
                                secondTextMsg='de forma ágil y segura.' />
                        </Fragment>
                    ) : (
                        <ButtonPayment
                            disabled={!isDisabled}
                            title={'Botón PSE'}
                            text={'Desde tu cuenta de ahorros o corriente'}
                            imgName={'imgPSE'}
                            onClick={() => handlerOnSubmit("PSE")} />
                    )
                    }
                </div>
            </div >
            <InformationModal
                isModalActivate={isModalActivate}
                hide={toggleModal}
                modalType={modalType}
            />
            <Footer />
        </>
    )
}
export default PaymentOption;
